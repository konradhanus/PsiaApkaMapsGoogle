using UnityEngine;

public class CubeGravityController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isPlayerInside;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        isPlayerInside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            isPlayerInside = true;
            rb.useGravity = true;
        }
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            rb.useGravity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Gem"))
        {
            isPlayerInside = false;
        }
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }
}