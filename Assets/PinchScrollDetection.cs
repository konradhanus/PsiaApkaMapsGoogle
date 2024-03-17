using UnityEngine;

public class InputManager : MonoBehaviour
{
    public float rotationSpeed = 0.1f;

  	private float rotationY = 0.0f;
    private void Update()
    {
        // Sprawdź przeciąganie palcem na urządzeniach mobilnych
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float deltaY = touch.deltaPosition.x * rotationSpeed;
                RotateObject(deltaY);
            }
        }

        // Sprawdź scrollowanie na myszce
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            RotateObject(scroll);
        }
    }

    void RotateObject(float deltaY)
    {
        // Zmieniaj rotację obiektu w osi Y na podstawie przesunięcia deltaY
       	float rotationAmount = deltaY;
        rotationY += rotationAmount;
        transform.rotation = Quaternion.Euler(25, rotationY, 0);
    }
}