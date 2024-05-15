using UnityEngine;

public class LimitRotation : MonoBehaviour
{
    public float maxRotation = 35f; // Maksymalna dozwolona rotacja w stopniach

    void Update()
    {
        // Pobierz rotację obiektu
        Vector3 currentRotation = transform.eulerAngles;

        // Ogranicz rotację w osi X
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxRotation, maxRotation);

        // Ustaw ograniczoną rotację z powrotem na obiekcie
        transform.eulerAngles = currentRotation;
    }
}
