using UnityEngine;
using TMPro; // Dodanie referencji do TextMeshPro
using System.Collections; // Dodanie referencji do System.Collections

public class DogController : MonoBehaviour
{

    public int actionType = 13;
    public bool switch3DModels = true;
    public GameObject ball; // GameObject piłki
    public GameObject ball2; // GameObject piłki
    public GameObject startPointDog; // GameObject pozycji startowej psa
    public float speed = 5f; // Prędkość poruszania się psa
    public float rotationSpeed = 5f; // Prędkość rotacji psa
    public float stopDistance = 2f; // Odległość, w której pies przestaje biec
    public float stopDistanceFromStart = 5f; // Odległość, w której pies przestaje biec do punktu startowego
    public TextMeshProUGUI distanceText; // Zmienna TextMeshPro do wyświetlania odległości

    private Rigidbody rb;
    private BallController ballController;
    private Animator animator;
    public bool hasReachedBall = false;

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
            float distance = Vector3.Distance(transform.position, ball.transform.position);
            UpdateDistanceText(distance); // Aktualizacja tekstu z odległością

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
                    Debug.Log("Odległość do piłki: " + distance);

                    if (distance > stopDistance)
                    {
                        // hasReachedBall = false;
                        
                        Debug.Log("Piłka dotknęła ziemi. Dodawanie siły: " + direction * speed);
                        // Dodaj siłę w kierunku piłki
                        rb.AddForce(direction * speed);
 
                        animator.SetFloat("Movement_f", 1f);
                     
                    }
                    else
                    {
                        Debug.Log("Piłka w pysku psa");
                        // hasReachedBall = true;
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        animator.SetFloat("Movement_f", 0f);
                        

                        // Debug.Log("Pies dotarł do piłki. Zatrzymanie ruchu i uruchomienie animacji 2.");
                        //StartCoroutine(HideAndShowBallCoroutine());

                        if (switch3DModels)
                        {
                            ball.SetActive(false);  // Ukryj piłkę 1
                            ball2.SetActive(true);  // Pokaz piłkę 2
                        }
                        StartCoroutine(ExecuteAction());

                    }
                }
                else
                {
                   animator.SetFloat("Movement_f", 0f);
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
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            animator.SetFloat("Movement_f", 0f);
            Debug.Log("Pies jest w odległości 5 metrów od punktu startowego. Zatrzymanie ruchu i animacji.");
        }
        else if (distanceToStart > stopDistance)
        {
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
            
            Debug.Log("Pies dotarł do pozycji startowej.");
    
            animator.SetInteger("ActionType_int", 0);
        }
    }

    void UpdateDistanceText(float distance)
    {
        if (distanceText != null)
        {
            distanceText.text = "Odległość do piłki: " + distance.ToString("F2") + " m";
        }
    }

    IEnumerator ExecuteAction()
    {
        animator.SetInteger("ActionType_int", actionType); // Uruchomienie akcji 13
        yield return new WaitForSeconds(1);
       
        animator.SetInteger("ActionType_int", 0); // Zmiana wartości na 0 po 1 sekundzie
    }


   


}
