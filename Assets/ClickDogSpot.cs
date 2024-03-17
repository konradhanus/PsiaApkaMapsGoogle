using UnityEngine;

public class TouchHandler : MonoBehaviour
{
 
    // Kolor docelowy
    public Color targetColor = Color.red;

    // Metoda wywoływana po dotknięciu ekranu
    private void Update()
    {
        // Sprawdź, czy na ekranie jest dotyk
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Pobierz pierwszy dotyk

            // Sprawdź, czy dotyk dotknął tego obiektu
            if (touch.phase == TouchPhase.Began && IsTouchingObject(touch.position))
            {
                // Zmiana koloru na docelowy
                GetComponent<Renderer>().material.color = targetColor;
                Debug.Log("klik");
            }
        }
    }

    // Metoda sprawdzająca, czy dotyk znajduje się nad obiektem
    private bool IsTouchingObject(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        // Sprawdź, czy promień dotyka obiektu
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }
}
