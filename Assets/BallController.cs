using UnityEngine;

public class BallController : MonoBehaviour
{
    public bool hasTouchedGround = false;
     public float groundHeight = 0.1f;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasTouchedGround = true;
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
