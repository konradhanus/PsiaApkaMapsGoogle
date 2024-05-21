using UnityEngine;

public class ChildPositionCorrection : MonoBehaviour
{
    public GameObject parentCube; // Poprawiona nazwa zmiennej

    private void Update()
    {
        // Sprawdź czy rodzic istnieje
        if (parentCube != null)
        {
            // Oblicz maksymalną skalę wzdłuż osi X i Y rodzica
            float maxParentScale = Mathf.Max(parentCube.transform.localScale.x, parentCube.transform.localScale.y);

            // Ustaw pozycję dziecka względem rodzica z ograniczeniem na maksymalną skalę
            transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, maxParentScale / 2f);

            // Zablokuj wartość osi Z dziecka na stałą wartość 1.8
            transform.localPosition = new Vector3(transform.localPosition.x, 0f, 0f);

            // Sprawdź czy dziecko jest wewnątrz rodzica
            if (!IsInsideParent())
            {
                // Jeśli nie, przenieś dziecko na granicę rodzica
                Vector3 correctedPosition = transform.localPosition.normalized * (maxParentScale / 2f);
                transform.localPosition = new Vector3(correctedPosition.x, 0f, 0f);
            }
        }
    }

    // Metoda sprawdzająca czy dziecko jest wewnątrz rodzica
    private bool IsInsideParent()
    {
        Vector3 parentScale = parentCube.transform.localScale / 2f;
        return Mathf.Abs(transform.localPosition.x) <= parentScale.x &&
               Mathf.Abs(transform.localPosition.y) <= parentScale.y;
    }
}
