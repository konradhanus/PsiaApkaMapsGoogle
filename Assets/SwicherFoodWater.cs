using UnityEngine;
using TMPro;

public class SwitcherFoodWater : MonoBehaviour
{
    public GameObject food;
    public GameObject foodCounter;
    public GameObject water;
    public GameObject waterCounter;
    // public TextMeshProUGUI text;
    public GameObject FoodOrWater;
    public GameObject Dog;

    public GameObject buttonWater;
    public GameObject buttonChicken;


    public bool isWaterActive = true;

    static bool isWaterActiveStatic = true;
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
        isWaterActiveStatic = !isWaterActiveStatic;
        // need this for BallController
        isWaterActive = isWaterActiveStatic;
        buttonWater.SetActive(!isWaterActiveStatic);
        buttonChicken.SetActive(isWaterActiveStatic);
        UpdateState();
    }

    private void UpdateState()
    {
        if (isWaterActiveStatic)
        {
            //text.text = "Jedzenie";
            food.SetActive(false);
            foodCounter.SetActive(false);
           

            water.SetActive(true);
            waterCounter.SetActive(true);
            if (rb != null)
            {
                rb.freezeRotation = true;
               
            }

            if (throwBallScript != null)
            {
                throwBallScript.freezeRotation = true;
                throwBallScript.isWater = true;
                throwBallScript.isChicken = false;
                throwBallScript.isBall = false;
            }

            if (dogController != null)
            {
                dogController.actionType = 7;
            }

        }
        else
        {
            //text.text = "Woda";
            food.SetActive(true);
            foodCounter.SetActive(true);
            water.SetActive(false);
            waterCounter.SetActive(false);

            if (rb != null)
            {
                rb.freezeRotation = false;
            }
            if (throwBallScript != null)
            {
                throwBallScript.freezeRotation = false;
                throwBallScript.isWater = false;
                throwBallScript.isChicken = true;
                throwBallScript.isBall = false;
            }
            if (dogController != null)
            {
                dogController.actionType = 5;
            }
        }
    }
}
