using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool hasTouchedGround = false;
    public float groundHeight = 0.1f;
    public SliderController sliderController; // Dodajemy referencję do SliderController


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasTouchedGround = true;

            if (sliderController != null)
            {
                sliderController.UpdateProgress(); // Wywołujemy metodę UpdateProgress

            }
            else {
                Debug.Log("progress nie ma");
            }

        }
    }

    void Update()
    {
        if (transform.position.y > groundHeight)
        {
            hasTouchedGround = false;
        }
    }
}
