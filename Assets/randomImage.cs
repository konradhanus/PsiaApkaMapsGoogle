using UnityEngine;
using UnityEngine.UI;

public class RandomImageSelector : MonoBehaviour
{
    // Publiczna lista sprite'ów do ustawienia w edytorze Unity
    public Sprite[] images;

    // Komponent Image przypięty do gameObjecta
    private Image imageComponent;

    // Statyczna zmienna przechowująca indeks wylosowanego obrazka
    private static int? selectedImageIndex = null;

    void Start()
    {
        // Pobieramy komponent Image przypięty do tego gameObjecta
        imageComponent = GetComponent<Image>();

        // Upewniamy się, że mamy przypisane jakieś obrazki
        if (images != null && images.Length > 0)
        {
            // Jeśli obrazek nie został jeszcze wylosowany, losujemy go
            if (selectedImageIndex == null)
            {
                selectedImageIndex = Random.Range(0, images.Length);
            }

            // Ustawiamy wylosowany obrazek
            imageComponent.sprite = images[selectedImageIndex.Value];
        }
        else
        {
            Debug.LogWarning("No images assigned to the RandomImageSelector script.");
        }
    }
}
