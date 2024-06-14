using UnityEngine;

public class WhenBackToScreen : MonoBehaviour
{
    public GameObject objectToDisable; // Obiekt, który ma być wyłączony

    private void OnEnable()
    {
       ClickDogSpot.ResetClick();
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false); // Wyłącz obiekt
        }
    }
}