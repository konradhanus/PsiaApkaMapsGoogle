using UnityEngine;

public class PanAndClamp : MonoBehaviour
{
    public float Speed;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 TouchDeltaPosition = Input.GetTouch(0).deltaPosition;
            transform.Translate(-TouchDeltaPosition.x * Speed, -TouchDeltaPosition.y * Speed, 0);

            Debug.Log(TouchDeltaPosition.x);
            Debug.Log(TouchDeltaPosition.y);
            
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, -40f, 40f),
                Mathf.Clamp(transform.position.y, 0f, 0f),
                Mathf.Clamp(transform.position.z, 25f, 25f)
            );
        }
    }
}
