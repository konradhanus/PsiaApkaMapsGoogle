using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Prędkość obrotu

    public float initialRotationOffset = 0f; // Początkowe przesunięcie rotacji

    void Start()
    {
        // Dodaj losowe przesunięcie do początkowej rotacji
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
