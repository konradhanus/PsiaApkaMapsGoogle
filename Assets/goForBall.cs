using UnityEngine;

public class DogController : MonoBehaviour
{
    public GameObject ball; // GameObject piłki
    public GameObject startPointDog; // GameObject pozycji startowej psa
    public float speed = 5f; // Prędkość poruszania się psa
    public float rotationSpeed = 5f; // Prędkość rotacji psa
    public float stopDistance = 1f; // Odległość, w której pies przestaje biec
    public float stopDistanceFromStart = 5f; // Odległość, w której pies przestaje biec do punktu startowego

    private Rigidbody rb;
    private BallController ballController;
    private Animator animator;
    private bool hasReachedBall = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (ball != null)
        {
            ballController = ball.GetComponent<BallController>();
        }
    }

    void Update()
    {
        if (ball != null)
        {
            if (!hasReachedBall)
            {
                // Oblicz kierunek w stronę piłki
                Vector3 direction = (ball.transform.position - transform.position).normalized;

                // Oblicz rotację w stronę piłki
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

                // Rotuj psa w stronę piłki
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                if (ballController != null && ballController.hasTouchedGround)
                {
                    float distance = Vector3.Distance(transform.position, ball.transform.position);
                    Debug.Log("Odległość do piłki: " + distance);

                    if (distance > stopDistance)
                    {
                        // animator.SetBool("Sit_b", false);
                        animator.SetFloat("Movement_f", 1f);
                        Debug.Log("Piłka dotknęła ziemi. Dodawanie siły: " + direction * speed);
                        // Dodaj siłę w kierunku piłki
                        
                        rb.AddForce(direction * speed);
                    }
                    else
                    {
                        hasReachedBall = true;
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        animator.SetFloat("Movement_f", 0f);
                        // animator.SetBool("Sit_b", true);
                        Debug.Log("Pies dotarł do piłki. Zatrzymanie ruchu i uruchomienie animacji 2.");
                    }
                }
                else
                {
                    // Zatrzymaj animację ruchu
                    animator.SetFloat("Movement_f", 0f);
                    Debug.Log("Piłka nie dotknęła jeszcze ziemi. Pies wraca do pozycji startowej.");
                    ReturnToStartPosition();
                }
            }
        }
    }

    void ReturnToStartPosition()
    {
        Vector3 startPosition = startPointDog.transform.position;
        Vector3 directionToStart = (startPosition - transform.position).normalized;
        float distanceToStart = Vector3.Distance(transform.position, startPosition);

        if (distanceToStart > stopDistanceFromStart)
        {
            // Zatrzymaj ruch i animację psa, gdy jest w odległości 5 metrów od punktu startowego
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            animator.SetFloat("Movement_f", 0f);
            Debug.Log("Pies jest w odległości 5 metrów od punktu startowego. Zatrzymanie ruchu i animacji.");
        }
        else if (distanceToStart > stopDistance)
        {
            // Oblicz rotację w stronę pozycji startowej
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToStart.x, 0, directionToStart.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

            animator.SetFloat("Movement_f", 1f);
            rb.AddForce(directionToStart * speed);
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            animator.SetFloat("Movement_f", 0f);
            // animator.SetBool("Sit_b", true);
            Debug.Log("Pies dotarł do pozycji startowej.");
        }
    }
}
