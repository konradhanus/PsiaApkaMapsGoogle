using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ButtonController : MonoBehaviour
{
    public Animator animator; // Referencja do animatora
    public bool isPressed = false;
    public bool isPressedJump = false; // Flaga określająca, czy przycisk jest wciśnięty
    private bool AttackReady_b = false; // Flaga określająca, czy atak jest gotowy

    public Button button; // Referencja do komponentu Button
    public Button buttonJump;

    public GameObject objectToIncreaseHeight;

    public float verticalSpeed = 1f; // Szybkość wzrostu wysokości obiektu
    public float maxHeight = 50f; // Maksymalna wysokość obiektu

    private void Start()
    {
        // Sprawdzenie, czy animator nie jest przypisany, jeśli nie, znajdź go w hierarchii
        if (animator == null)
            animator = GetComponent<Animator>();

        // Dodanie funkcji do zdarzeń wciśnięcia i zwolnienia przycisku RUN
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        AddEventTrigger(trigger, EventTriggerType.PointerDown, OnPointerDown);
        AddEventTrigger(trigger, EventTriggerType.PointerUp, OnPointerUp);

        // Dodanie funkcji do zdarzeń wciśnięcia i zwolnienia przycisku JUMP
        EventTrigger triggerJump = buttonJump.gameObject.AddComponent<EventTrigger>();
        AddEventTrigger(triggerJump, EventTriggerType.PointerDown, OnPointerDownJump);
        AddEventTrigger(triggerJump, EventTriggerType.PointerUp, OnPointerUpJump);
    }

    void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback.AddListener((data) => { action.Invoke((BaseEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void OnPointerDown(BaseEventData eventData)
    {
        // Ustawienie flagi na true, gdy przycisk jest wciśnięty
        isPressed = true;
        Debug.Log("PRAWDA");

        // Ustawienie wartości parametru w animatorze
        //animator.SetBool("Sit_b", isPressed);
         animator.SetFloat("Movement_f", 1f);
    }

    public void OnPointerDownJump(BaseEventData eventData)
    {
        // Ustawienie flagi na true, gdy przycisk jest wciśnięty
        Invoke("ChangeIsPressedJump", 0.1f);
        Debug.Log("PRAWDA JUMP");

        // Ustawienie wartości parametru w animatorze
        //animator.SetBool("Sit_b", isPressed);
        Jump();
    }


    private void ChangeIsPressedJump()
    {
        // Po upływie jednej sekundy zmień wartość isPressedJump na true
        isPressedJump = true;
        Debug.Log("PRAWDA JUMP");
    }

    private void Update()
    {
        if (isPressedJump)
        {
            Debug.Log("jumpx");
            IncreaseHeight(objectToIncreaseHeight);
        }
    }

    void IncreaseHeight(GameObject objToIncrease)
    {
        Debug.Log("increase");
        // Upewnij się, że obiekt nie jest null
        if (objToIncrease != null)
        {
            // Zwiększanie wysokości obiektu o niewielką wartość
            Vector3 newPosition = objToIncrease.transform.position;
            newPosition.y += verticalSpeed * Time.deltaTime;
            // Sprawdzenie, czy nowa wysokość nie przekracza maksymalnej
            newPosition.y = Mathf.Min(newPosition.y, maxHeight);
            objToIncrease.transform.position = newPosition;
        }
    }
    public void OnPointerUp(BaseEventData eventData)
    {
        // Ustawienie flagi na false, gdy przycisk jest zwolniony
        isPressed = false;
        Debug.Log("FAŁSZ");

        // Ustawienie wartości parametru w animatorze
        animator.SetFloat("Movement_f", 0f);

        // Ustawienie wartości parametru w animatorze
        //animator.SetBool("Sit_b", isPressed);
    }

    public void OnPointerUpJump(BaseEventData eventData)
    {
        // Ustawienie flagi na false, gdy przycisk jest zwolniony
        isPressedJump = false;
        Debug.Log("FAŁSZ JUMP");

        // Ustawienie wartości parametru w animatorze
       

        // Ustawienie wartości parametru w animatorze
        //animator.SetBool("Sit_b", isPressed);
    }

    public void Jump()
    {
        // isPressedJump = true;
        animator.SetTrigger("Jump_tr");
    }


    public void Attack()
    {
        // Ustawienie flagi na true na pół sekundy
        //  animator.SetBool("AttackReady_b", true);
        StartCoroutine(EnableAttackForHalfSecond());

        // Tutaj można umieścić kod odpowiedzialny za atak
    }

    private IEnumerator EnableAttackForHalfSecond()
    {

        animator.SetBool("AttackReady_b", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("AttackReady_b", false);
    }
    
}
