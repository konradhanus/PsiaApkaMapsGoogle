using UnityEngine;

public class dogAction : MonoBehaviour
{
    private Animator animator; // Zmienna do przechowywania referencji do animatora
    private bool isAnimating; // Zmienna określająca, czy aktualnie jest odtwarzana animacja

    void Start()
    {
        // Sprawdź, czy obiekt ma przypisanego animatora
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>(); // Przypisz animator do zmiennej
            // Ustaw parametr "Sleep_b" na true
          //  animator.SetBool("Sleep_b", true);
        }
        else
        {
            Debug.LogError("Obiekt nie ma przypisanego animatora.");
        }
    }

    // Funkcja wykonująca akcję siadania
    public void Siad()
    {
        if (animator != null)
        {
            // Ustaw parametr "Sit_b" na true
            animator.SetBool("Sit_b", true);
            // Zeruj pozostałe parametry
            animator.SetBool("Sleep_b", false);
            animator.SetInteger("ActionType_int", 0);
        }
    }

    // Funkcja wykonująca akcję poproszenia
    public void Popros()
    {
        if (animator != null && !isAnimating)
        {
            // Ustaw parametr "ActionType_int" na 2
            animator.SetInteger("ActionType_int", 2);
            // Zeruj pozostałe parametry
            animator.SetBool("Sleep_b", false);
            animator.SetBool("Sit_b", false);
            // Ustaw flagę animacji na true
            isAnimating = true;
        }
    }

    // Funkcja wykonująca akcję idź spać (używając parametru Sleep_b)
    public void IdzSpac()
    {
        if (animator != null)
        {
            // Ustaw parametr "Sleep_b" na true
            animator.SetBool("Sleep_b", true);
            // Zeruj pozostałe parametry
            animator.SetBool("Sit_b", false);
            animator.SetInteger("ActionType_int", 0);
        }
    }

    // Funkcja wykonująca akcję daj głos
    public void DajGlos()
    {
        if (animator != null && !isAnimating)
        {
            // Ustaw parametr "ActionType_int" na 1
            animator.SetInteger("ActionType_int", 1);
            // Zeruj pozostałe parametry
            animator.SetBool("Sleep_b", false);
            animator.SetBool("Sit_b", false);
            // Ustaw flagę animacji na true
            isAnimating = true;
        }
    }

    // Funkcja wywoływana po zakończeniu animacji
    public void AnimationFinished()
    {
        isAnimating = false;
        // Wróć do stanu idle
        animator.SetInteger("ActionType_int", 0);
    }
}
