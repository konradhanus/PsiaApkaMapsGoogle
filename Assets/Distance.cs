using UnityEngine;
using TMPro;

public class UpdateZPositionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro; // Referencja do komponentu TextMeshPro

    private void Update()
    {
        // Pobierz bieżącą pozycję obiektu na osi Z
        float currentZPosition = transform.position.z;

        // Zaokrąglij pozycję do liczby całkowitej
        int roundedZPosition = Mathf.RoundToInt(currentZPosition);

        // Zaktualizuj tekst w TextMeshPro
        textMeshPro.text = "" + roundedZPosition.ToString() +" m";
    }
}
