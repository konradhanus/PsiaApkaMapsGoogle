using UnityEngine;

public class Dog : MonoBehaviour
{
    public Transform ball;  // Referencja do obiektu piłki
    public float speed = 5.0f;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
            // Pies biegnie za piłką, ale tylko w płaszczyźnie XZ
            Vector3 direction = (ball.position - transform.position).normalized;
            direction.y = 0; // Upewnij się, że pies nie zmienia wysokości

            // Porusz psa używając AddForce
            rb.AddForce(direction * speed, ForceMode.VelocityChange);

            // Ustaw rotację psa w kierunku piłki w osi Y
            Vector3 lookDirection = ball.position - transform.position;
            lookDirection.y = 0; // Ignoruj różnicę wysokości
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                rb.MoveRotation(targetRotation);
            }

            // Uruchom animację biegu
            animator.SetFloat("Movement_f", 0.5f);
       
    }
}
