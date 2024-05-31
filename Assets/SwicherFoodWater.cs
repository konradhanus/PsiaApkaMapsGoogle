using UnityEngine;
using TMPro;

public class SwitcherFoodWater : MonoBehaviour
{
    public GameObject food;
    public GameObject water;
    public TextMeshProUGUI text;
    public GameObject FoodOrWater;
    public GameObject Dog;


    public bool isWaterActive = true;
    private Rigidbody rb;
    private ThrowBall throwBallScript;
    private DogController dogController;

    void Start()
    {
        if (FoodOrWater != null)
        {
            rb = FoodOrWater.GetComponent<Rigidbody>();
            throwBallScript = FoodOrWater.GetComponent<ThrowBall>();
        }

        if (Dog != null)
        {
            dogController = Dog.GetComponent<DogController>();
        }

        UpdateState();
    }

    public void SwitchButton()
    {
        isWaterActive = !isWaterActive;
        UpdateState();
    }

    private void UpdateState()
    {
        if (isWaterActive)
        {
            text.text = "Jedzenie";
            food.SetActive(false);
            water.SetActive(true);
            if (rb != null)
            {
                rb.freezeRotation = true;
            }

            if (throwBallScript != null)
            {
                throwBallScript.freezeRotation = true;
            }

            if (dogController != null)
            {
                dogController.actionType = 7;
            }

        }
        else
        {
            text.text = "Woda";
            food.SetActive(true);
            water.SetActive(false);
            if (rb != null)
            {
                rb.freezeRotation = false;
            }
            if (throwBallScript != null)
            {
                throwBallScript.freezeRotation = false;
            }
            if (dogController != null)
            {
                dogController.actionType = 5;
            }
        }
    }
}
