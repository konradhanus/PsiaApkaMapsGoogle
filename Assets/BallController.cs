using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool hasTouchedGround = false;
    public float groundHeight = 0.1f;
    public SliderController hungerSliderController; // Dodajemy referencję do SliderController
    public SliderController tirstySliderController; // Dodajemy referencję do SliderController
    public GameObject ButtonStatus;

    private SwitcherFoodWater buttonStatus;


    void Start()
    {
        if (ButtonStatus != null)
        {
            buttonStatus = ButtonStatus.GetComponent<SwitcherFoodWater>();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasTouchedGround = true;

            if (hungerSliderController != null && buttonStatus.gameObject.activeInHierarchy)
            {
                hungerSliderController.UpdateProgress(); // Wywołujemy metodę UpdateProgress
                Debug.Log("HUNDER");

            }

            if (tirstySliderController != null && !buttonStatus.gameObject.activeInHierarchy)
            {
                tirstySliderController.UpdateProgress(); // Wywołujemy metodę UpdateProgress
                Debug.Log("HUNDER _ PICIE");

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
