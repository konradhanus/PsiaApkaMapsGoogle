using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{

    public GameObject ball; // GameObject piłki
    public GameObject ball2; // GameObject piłki
    float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 previousMousePos;
    private Vector2 currentMousePos;
    private float rotationSpeed = 10f;
    public bool isBall = true;
    public bool isWater = false;
    public bool isChicken = false;
    public GameObject callbackGameObject; // Nowy parametr typu GameObject

    public float MinSwipDist = 0;
    private float BallVelocity = 0;
    private float BallSpeed = 0;
    public float MaxBallSpeed = 350;
    private Vector3 angle;
    public bool freezeRotation = false; // Nowy parametr
    private bool thrown, holding;
    private Vector3 newPosition, resetPos;
    private Vector3 currentRotationSpeed;
    private Vector3 rotationDamping = new Vector3(0.95f, 0.95f, 0.95f); // Damping factor for rotation

    public GameObject BallValue;

    public GameObject ballObject;

    Rigidbody rb;
    public Animator dogAnimator; // Dodanie Animatora psa

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        resetPos = transform.position;
        ResetBall();
    }

    private void OnMouseDown()
    {
        startTime = Time.time;
        startPos = Input.mousePosition;
        previousMousePos = startPos;
        holding = true;
        if (dogAnimator != null)
        {
            dogAnimator.SetBool("AttackReady_b", true);
        }
    }

    private void OnMouseDrag()
    {
        PickupBall();
        if (!freezeRotation)
        {
            RotateBall();
        }
    }

    private void OnMouseUp()
    {
        endTime = Time.time;
        endPos = Input.mousePosition;
        swipeDistance = (endPos - startPos).magnitude;
        swipeTime = endTime - startTime;
        
        // Wywołaj metodę callback
        if (callbackGameObject != null)
        {
            GetFoodData getFoodData = callbackGameObject.GetComponent<GetFoodData>();
            GetBallNew getBall = callbackGameObject.GetComponent<GetBallNew>();

            if(getBall != null)
            { 
                if (isBall)
                {
                    getBall.StartCoroutine(getBall.UpdateResource("ball", -1));
                }
            }

            if (getFoodData != null)
            {
              

                if (isWater)
                {
                    getFoodData.StartCoroutine(getFoodData.UpdateResource("water", -1));
                }

                if (isChicken)
                {
                    getFoodData.StartCoroutine(getFoodData.UpdateResource("chicken", -1));
                }

                Debug.Log("FETCH");
            }
            else {
                Debug.Log("NO FETCH");
            }
        }

        

        if (swipeDistance > 30f)
        {
            //throw ball
            CalSpeed();
            CalAngle();
            Debug.Log(angle);
            rb.AddForce(new Vector3((angle.x * BallSpeed * 2 * 2), (angle.y * BallSpeed ), (-angle.z * BallSpeed * 2)));
            if (!freezeRotation)
            {
                rb.angularVelocity = currentRotationSpeed; // Preserve rotational velocity
            }
            rb.useGravity = true;
            holding = false;
            thrown = true;

            // Ustawienie parametru animacji psa
            if (dogAnimator != null)
            {
                dogAnimator.SetBool("AttackReady_b", false);
            }

            Invoke("ResetBall", 5f);
        }
        else
        {
            ResetBall();
          
            
        }

        // Pobierz komponent BallScript z ballObject
        GetBallNew ballScript = BallValue.GetComponent<GetBallNew>();

        // Sprawdź, czy skrypt został znaleziony
        if (ballScript != null)
        {
            // Odczytaj wartość ballValue
            int value = ballScript.ballValue;
            Debug.Log("BALL ::" + value);

            if (value <= 0)
            {
                ball.SetActive(false);
                ball2.SetActive(false);// urkyj piłkę 1
            }

            Debug.Log("Wartość ballValue: " + value);
        }
        else
        {
            Debug.LogError("Nie znaleziono skryptu BallScript na obiekcie " + ballObject.name);
        }
    }

    public void ResetBall()
    {
        Taptic.Success();

        Debug.Log("Reset Ball from ResetBall");
        angle = Vector3.zero;
        endPos = Vector2.zero;
        startPos = Vector2.zero;
        BallSpeed = 0;
        startTime = 0;
        endTime = 0;
        swipeDistance = 0;
        swipeTime = 0;
        thrown = holding = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero; // Reset rotation velocity
        rb.useGravity = false;
        transform.position = resetPos;
        transform.rotation = Quaternion.identity; // Reset rotation
        currentRotationSpeed = Vector3.zero; // Reset rotation speed
        
        ball.SetActive(true);  // pokaz piłkę 1
        ball2.SetActive(false);  // Ukryj piłkę 2

       

    }

    void PickupBall()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane * 15f;
        newPosition = Camera.main.ScreenToWorldPoint(mousePos);
        transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, 80f * Time.deltaTime);
    }

    void RotateBall()
    {
        currentMousePos = Input.mousePosition;
        Vector2 deltaMousePos = currentMousePos - previousMousePos;

        float rotationX = (deltaMousePos.y * rotationSpeed * Time.deltaTime) / 4; // 4 times slower
        float rotationY = (-deltaMousePos.x * rotationSpeed * Time.deltaTime) / 4; // 4 times slower

        currentRotationSpeed += new Vector3(rotationX, rotationY, 0);

        transform.Rotate(Camera.main.transform.up, currentRotationSpeed.y, Space.World);
        transform.Rotate(Camera.main.transform.right, currentRotationSpeed.x, Space.World);

        previousMousePos = currentMousePos;
    }

    private void Update()
    {
        if (!holding && !freezeRotation)
        {
            // Apply damping to slow down rotation
            currentRotationSpeed = Vector3.Scale(currentRotationSpeed, rotationDamping);
        }

        if (holding)
        {
            PickupBall();
        }
    }

    private void CalAngle()
    {
        angle = Camera.main.ScreenToWorldPoint(new Vector3(endPos.x + 50, endPos.y, (Camera.main.nearClipPlane + 5)));
    }

    void CalSpeed()
    {
        if (swipeTime > 0)
            BallVelocity = swipeDistance / (swipeDistance - swipeTime);

        BallSpeed = BallVelocity * 4;

        if (BallSpeed <= MaxBallSpeed)
        {
            BallSpeed = MaxBallSpeed;
        }
        swipeTime = 0;
    }


}
