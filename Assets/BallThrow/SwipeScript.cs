using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SwipeScript : MonoBehaviour
{
    Vector2 startPos, endPos, direction; // touch start position, touch end position, swipe direction
    float touchTimeStart, touchTimeFinish, timeInterval; // to calculate swipe time to control throw force in Z direction

    [SerializeField]
    float throwForceInXandY = 1f; // to control throw force in X and Y directions

    [SerializeField]
    float throwForceInZ = 50f; // to control throw force in Z direction

    [SerializeField]
    TextMeshProUGUI xForceText; // TextMeshPro for displaying X force

    [SerializeField]
    TextMeshProUGUI yForceText; // TextMeshPro for displaying Y force

    [SerializeField]
    TextMeshProUGUI zForceText; // TextMeshPro for displaying Z force

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // if you touch the screen
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // getting touch position and marking time when you touch the screen
            touchTimeStart = Time.time;
            startPos = Input.GetTouch(0).position;
        }

        // if you release your finger
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // marking time when you release it
            touchTimeFinish = Time.time;

            // calculate swipe time interval 
            timeInterval = touchTimeFinish - touchTimeStart;
            Debug.Log("Original Interval: " + timeInterval);

            // Scale the time interval to be within 0.01 to 0.2 using a logarithmic scale
            float minInterval = 0.05f;
            float maxInterval = 0.13f;
            float minSwipeTime = 0.1f; // minimum swipe time in seconds
            float maxSwipeTime = 5.0f; // maximum swipe time in seconds
            timeInterval = Mathf.Clamp(timeInterval, minSwipeTime, maxSwipeTime);
            float scaledTimeInterval = minInterval + (maxInterval - minInterval) * (Mathf.Log(timeInterval / minSwipeTime) / Mathf.Log(maxSwipeTime / minSwipeTime));
            Debug.Log("Scaled Interval: " + scaledTimeInterval);

            // getting release finger position
            endPos = Input.GetTouch(0).position;

            // calculating swipe direction in 2D space
            direction = endPos - startPos; // Changed from startPos - endPos to endPos - startPos
            Debug.Log("Direction: " + direction);

            // calculate forces
            float xForce = direction.x * throwForceInXandY;
            float yForce = direction.y * throwForceInXandY;
            float zForce = throwForceInZ ;

            // update TextMeshPro fields
           

            // add force to ball's rigidbody in 3D space depending on swipe time, direction and throw forces
            rb.isKinematic = false;
            Debug.Log("POWER: " + xForce + ", " + yForce + ", " + zForce);
            rb.AddForce(xForce, yForce, zForce);

            // Destroy ball in 4 seconds
            Destroy(gameObject, 3f);

            xForceText.text = "X Force: " + xForce.ToString("F2");
            yForceText.text = "Y Force: " + yForce.ToString("F2");
            zForceText.text = "Z Force: " + zForce.ToString("F2");
        }
    }
}
