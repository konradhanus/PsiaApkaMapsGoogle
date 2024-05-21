using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 30f; // Dodano siłę skoku
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public Animator animator;
    private Vector2 moveInput;
    private Rigidbody rb;

    private float lastJumpTime; // Zmienna do przechowywania czasu ostatniego skoku
    private float jumpCooldown = 1f; // Czas, jaki musi upłynąć między skokami

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lastJumpTime = -jumpCooldown; // Inicjalizacja, aby umożliwić skok na początku
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable(); // Włącz akcję skoku
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;
        jumpAction.action.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable(); // Wyłącz akcję skoku
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        jumpAction.action.performed -= OnJumpPerformed;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (transform.position.y < 1 && Time.time >= lastJumpTime + jumpCooldown) // Sprawdź, czy pozycja gracza na osi Y jest mniejsza niż 1 i czy minął odpowiedni czas
        {
            animator.SetTrigger("Jump_tr"); // Uruchom animację skoku
            StartCoroutine(JumpWithDelay(0.4f));
            lastJumpTime = Time.time; // Zaktualizuj czas ostatniego skoku
        }
    }

    private IEnumerator JumpWithDelay(float delay)
    {
        // Poczekaj określoną ilość czasu (w sekundach)
        yield return new WaitForSeconds(delay);

        // Dodaj siłę skoku po upływie czasu
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("Movement_f", 1f);
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        animator.SetFloat("Movement_f", 0f);
    }

    private void Update()
    {
        if (moveInput.x < 0)
        {
            moveInput.x = 0;
        }

        transform.position = new Vector3(0, transform.position.y, transform.position.z);

        Vector3 movement = new Vector3(0, 0, moveInput.x) * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
