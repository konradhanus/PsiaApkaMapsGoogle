using System.Collections;
using UnityEngine;

public class dogAction : MonoBehaviour
{
    private Animator animator; // Zmienna do przechowywania referencji do animatora
    private bool isAnimating; // Zmienna określająca, czy aktualnie jest odtwarzana animacja
    public GameObject commandsMenu;

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

    public void Idle()
    {
        if (animator != null)
        {
            animator.SetBool("Sit_b", false);
            animator.SetBool("Sleep_b", false);
            animator.SetInteger("ActionType_int", 0);
        }
    }

    // Funkcja wykonująca akcję poproszenia
    public void Popros()
    {
        if (animator != null)
        {
            // Ustaw parametr "ActionType_int" na 2
            animator.SetInteger("ActionType_int", 2);
            // Zeruj pozostałe parametry
            animator.SetBool("Sleep_b", false);
            animator.SetBool("Sit_b", false);
            // Ustaw flagę animacji na true
            StartCoroutine(LongDelay());

        }
    }

    // Funkcja wykonująca akcję idź spać (używając parametru Sleep_b)
    public void IdzSpac()
    {
        if (animator != null)
        {
            // Ustaw parametr "Sleep_b" na true
            animator.SetBool("Sleep_b", true);
            animator.SetInteger("ActionType_int", 0);
        }
    }

    public void PodajLape()
    {
        if (animator != null)
        {
            // Ustaw parametr "Sleep_b" na true
            animator.SetBool("Sleep_b", false);
            animator.SetBool("Sit_b", true);
            animator.SetInteger("ActionType_int", 2);
            StartCoroutine(LongDelay());
        }
    }

    public void NiedobryPies()
    {
        if (animator != null)
        {
            // Ustaw parametr "Sleep_b" na true
            animator.SetBool("Sleep_b", false);
            animator.SetBool("Sit_b", false);
            animator.SetInteger("ActionType_int", 3);
            StartCoroutine(LongDelay());
        }
    }

  

    // Funkcja wykonująca akcję daj głos
    public void DajGlos()
    {
        if (animator != null)
        {
            // Ustaw parametr "ActionType_int" na 1
            animator.SetInteger("ActionType_int", 1);
            // Zeruj pozostałe parametry
            animator.SetBool("Sleep_b", false);
            // Ustaw flagę animacji na true
            StartCoroutine(LongDelay());
        }
    }

    public void Kop()
    {
        if (animator != null)
        {
            animator.SetBool("Sleep_b", false);
            animator.SetBool("Sit_b", false);
            // Ustaw parametr "ActionType_int" na 1
            animator.SetInteger("ActionType_int",4);
            // Zeruj pozostałe parametry
            // Ustaw flagę animacji na true
            StartCoroutine(LongDelay());
        }
    }


    public void ToogleCommands()
    {
        // Sprawdź, czy obiekt jest null
        if (commandsMenu != null)
        {
            // Jeśli obiekt jest aktywny (widoczny), ukryj go
            if (commandsMenu.activeSelf)
            {
                commandsMenu.SetActive(false); // Ukryj obiekt
            }
            else // W przeciwnym razie pokaż obiekt
            {
                commandsMenu.SetActive(true); // Pokaż obiekt
            }
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!"); // Wyświetl ostrzeżenie, jeśli obiekt nie został przypisany
        }
    }
    IEnumerator LongDelay()
    {
        // Poczekaj 1 sekundę
        yield return new WaitForSeconds(4f);

        // Ustaw action type na 0
        animator.SetInteger("ActionType_int", 0);
    }
}
