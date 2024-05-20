using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 70f; // Dodano siłę skoku
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    public Animator animator;
    private Vector2 moveInput;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable(); // Włącz akcję skoku
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;
        jumpAction.action.performed += OnJumpPerformed; // S
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable(); // Wyłącz akcję skoku
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
        jumpAction.action.performed -= OnJumpPerformed; // Ods
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
       // if (Mathf.Approximately(rb.velocity.y, 0)) // Sprawdź, czy gracz jest na ziemi
       // {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        animator.SetTrigger("Jump_tr"); // Uruchom animację skoku

        // }
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
