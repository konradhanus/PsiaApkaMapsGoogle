using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidGenerator : MonoBehaviour
{
    public GameObject squarePrefab; // Prefab bloku o rozmiarze 1x1x1 z Rigidbody
    public int rows = 10; // Liczba wierszy piramidy

    void Start()
    {
        GeneratePyramid(rows);
    }

    void GeneratePyramid(int rows)
    {
        System.Random random = new System.Random();

        for (int i = 0; i < rows; i++)
        {
            // Liczba spacji przed blokami
            int spaces = rows - i - 1;
            // Liczba bloków w danym wierszu
            int blocks = 2 * i + 1;

            // Generowanie wiersza piramidy
            for (int j = 0; j < blocks; j++)
            {
                if (j == 0 || j == blocks - 1 || i == rows - 1 || random.Next(2) == 1)
                {
                    // Obliczanie pozycji bloku
                    float xPos = j - i;
                    float yPos = rows - i - 1;
                    Vector3 position = new Vector3(xPos, yPos, 0);

                    // Tworzenie bloku w pozycji
                    Instantiate(squarePrefab, position, Quaternion.identity);
                }
            }
        }
    }
}
