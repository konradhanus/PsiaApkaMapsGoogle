using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject imageToHide;  // Przypisz Image z Inspector
    public Scrollbar scrollbar;  // Przypisz Scrollbar z Inspector

    private float elapsedTime = 0f;
    private bool isImageHidden = false;

    void Start()
    {
        if (imageToHide == null || scrollbar == null)
        {
            Debug.LogError("Image or Scrollbar is not assigned in the Inspector.");
            return;
        }

        // Ustaw początkowy stan scrollbar'a na 0
        scrollbar.size = 0f;
    }

    void Update()
    {
        // Dodawanie czasu od ostatniej ramki
        elapsedTime += Time.deltaTime;

        // Sprawdzenie, czy minęło 5 sekund
        if (elapsedTime >= 5f && !isImageHidden)
        {
            // Ustawianie Scrollbar na 100%
            scrollbar.size = 1f;

            // Ukrywanie obrazu
            imageToHide.gameObject.SetActive(false);
            
            // Oznacz, że obraz został ukryty, aby uniknąć wielokrotnego ustawiania tych samych wartości
            isImageHidden = true;
        }
        else if (!isImageHidden)
        {
            // Proporcjonalne wypełnianie scrollbara
            scrollbar.size = elapsedTime / 3f;
        }
    }
}
