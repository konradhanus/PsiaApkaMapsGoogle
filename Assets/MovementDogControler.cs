using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour
{
    public Animator animator; // Referencja do animatora
    public bool isPressed = false; // Flaga określająca, czy przycisk jest wciśnięty

    public Button button; // Referencja do komponentu Button

    private void Start()
    {
        // Sprawdzenie, czy animator nie jest przypisany, jeśli nie, znajdź go w hierarchii
        if (animator == null)
            animator = GetComponent<Animator>();

        // Dodanie funkcji do zdarzeń wciśnięcia i zwolnienia przycisku
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        AddEventTrigger(trigger, EventTriggerType.PointerDown, OnPointerDown);
        AddEventTrigger(trigger, EventTriggerType.PointerUp, OnPointerUp);
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
}
