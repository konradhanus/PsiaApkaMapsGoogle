using System.Collections;
using UnityEngine;

public class RingRotation : MonoBehaviour
{
    // Czas potrzebny na pełną rotację (domyślnie 1 sekunda)
    public float rotationDuration = 1.0f;

    private float rotationSpeed;

    void Start()
    {
        // Obliczanie prędkości rotacji (360 stopni na czas ustawiony w rotationDuration)
        rotationSpeed = 360f / rotationDuration;
    }

    void Update()
    {
        // Obracanie obiektu wokół osi Y w czasie rzeczywistym
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
