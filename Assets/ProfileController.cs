using UnityEngine;

public class ProfileController : MonoBehaviour
{
    // Funkcja do ukrywania elementu
    public void HideElement(GameObject element)
    {
        if (element != null)
        {
            element.SetActive(false);
            Debug.Log("aktywny");
        }
        else
        {
            Debug.LogWarning("Element to hide is null!");
        }
    }

    // Funkcja do pokazywania elementu
    public void ShowElement(GameObject element)
    {
        if (element != null)
        {
            element.SetActive(true);
             Debug.Log("nie aktywny");
        }
        else
        {
            Debug.LogWarning("Element to show is null!");
        }
    }
}
