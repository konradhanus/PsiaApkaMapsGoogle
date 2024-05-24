using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    float startTime, endTime, swipeDistance, swipeTime;
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 previousMousePos;
    private Vector2 currentMousePos;
    private float rotationSpeed = 10f;

    public float MinSwipDist = 0;
    private float BallVelocity = 0;
    private float BallSpeed = 0;
    public float MaxBallSpeed = 350;
    private Vector3 angle;

    private bool thrown, holding;
    private Vector3 newPosition, resetPos;
    private Vector3 currentRotationSpeed;
    private Vector3 rotationDamping = new Vector3(0.95f, 0.95f, 0.95f); // Damping factor for rotation

    Rigidbody rb;

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
    }

    private void OnMouseDrag()
    {
        PickupBall();
        RotateBall();
    }

    private void OnMouseUp()
    {
        endTime = Time.time;
        endPos = Input.mousePosition;
        swipeDistance = (endPos - startPos).magnitude;
        swipeTime = endTime - startTime;

        if (swipeDistance > 30f)
        {
            //throw ball
            CalSpeed();
            CalAngle();
            Debug.Log(angle);
            rb.AddForce(new Vector3((angle.x * BallSpeed * 2 * 2), (angle.y * BallSpeed * 1.5f), (-angle.z * BallSpeed * 2)));
            rb.angularVelocity = currentRotationSpeed; // Preserve rotational velocity
            rb.useGravity = true;
            holding = false;
            thrown = true;
            Invoke("ResetBall", 4f);
        }
        else
        {
            ResetBall();
        }
    }

    void ResetBall()
    {
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
        if (!holding)
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

        BallSpeed = BallVelocity * 40;

        if (BallSpeed <= MaxBallSpeed)
        {
            BallSpeed = MaxBallSpeed;
        }
        swipeTime = 0;
    }
}
