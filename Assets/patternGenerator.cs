using System;
using UnityEngine;

public class PatternGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab cube
    public int mapLength = 100; // Długość mapy, którą chcemy wygenerować
    public float spacing = 1f; // Odstęp między obiektami

    public int minPlatformLength = 4; // Minimalna długość platformy
    public int maxPlatformLength = 9; // Maksymalna długość platformy
    public int minGapLength = 1; // Minimalna długość przerwy
    public int maxGapLength = 8; // Maksymalna długość przerwy

    void Start()
    {
        ValidateParameters();

        string worldMap = GenerateWorldMap(mapLength);
        Debug.Log("Generated World Map: " + worldMap);
        GeneratePattern(worldMap);
    }

    void ValidateParameters()
    {
        if (minPlatformLength > maxPlatformLength)
        {
            Debug.LogWarning("minPlatformLength is greater than maxPlatformLength. Adjusting values.");
            minPlatformLength = maxPlatformLength;
        }

        if (minGapLength > maxGapLength)
        {
            Debug.LogWarning("minGapLength is greater than maxGapLength. Adjusting values.");
            minGapLength = maxGapLength;
        }

        // Ensure that the platform lengths and gap lengths are within a sensible range
        minPlatformLength = Mathf.Clamp(minPlatformLength, 1, mapLength);
        maxPlatformLength = Mathf.Clamp(maxPlatformLength, minPlatformLength, mapLength);
        minGapLength = Mathf.Clamp(minGapLength, 0, mapLength);
        maxGapLength = Mathf.Clamp(maxGapLength, minGapLength, mapLength);
    }

    void GeneratePattern(string pattern)
    {
        Vector3 startPosition = transform.position; // Początkowa pozycja
        Vector3 currentPosition = startPosition;

        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == 'x')
            {
                Instantiate(cubePrefab, currentPosition, Quaternion.identity);
            }
            currentPosition.z += spacing; // Przesunięcie w osi Z
        }
    }

    private string GenerateWorldMap(int length)
    {
        System.Text.StringBuilder worldMap = new System.Text.StringBuilder();
        System.Random random = new System.Random();

        while (worldMap.Length < length)
        {
            // Generowanie platformy
            int remainingLength = length - worldMap.Length;
            int platformMinLength = Mathf.Min(minPlatformLength, remainingLength);
            int platformMaxLength = Mathf.Min(maxPlatformLength + 1, remainingLength + 1);

            if (platformMinLength >= platformMaxLength)
            {
                platformMaxLength = platformMinLength + 1;
            }

            int platformLength = random.Next(platformMinLength, platformMaxLength);
            worldMap.Append(new string('x', platformLength));

            // Sprawdzenie, czy jest miejsce na przerwę
            if (worldMap.Length >= length)
                break;

            // Generowanie przerwy
            remainingLength = length - worldMap.Length;
            int gapMinLength = Mathf.Min(minGapLength, remainingLength);
            int gapMaxLength = Mathf.Min(maxGapLength + 1, remainingLength + 1);

            if (gapMinLength >= gapMaxLength)
            {
                gapMaxLength = gapMinLength + 1;
            }

            int gapLength = random.Next(gapMinLength, gapMaxLength);
            worldMap.Append(new string('_', gapLength));
        }

        // Ucięcie nadmiarowych znaków, jeśli długość mapy została przekroczona
        if (worldMap.Length > length)
        {
            worldMap.Length = length;
        }

        return worldMap.ToString();
    }
}
