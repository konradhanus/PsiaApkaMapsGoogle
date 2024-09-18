using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;

public class AnimateDogCharacter : MonoBehaviour
{
    Animator animator;
    Vector3 initialPosition; // Zapisanie początkowej pozycji postaci
    void Start()
    {
        animator = GetComponent<Animator>();

        // Sprawdzenie, czy Animator istnieje
        if (animator == null)
        {
            Debug.LogError("WARTOSC Animator not found on GameObject!");
            return;
        }

        if (!HasParameter(animator, "Movement_f"))
        {
            Debug.LogError("WARTOSC Animator parameter 'Movement_f' not found!");
        }

        initialPosition = transform.position;

        // Testowe ustawienie wartości
        //animator.SetFloat("Movement_f", 3.5f);
        Debug.Log("WARTOSC2 Animator and parameters checked, setting initial values.");
    }

    void Update()
    {
        // Blokada pozycji postaci
        transform.position = initialPosition;
    }

    public void OnAnimationStateChange(MoveAvatar.AvatarAnimationState animationState)
    {
        Debug.Log("State changed to: " + animationState);

        switch (animationState)
        {
            case MoveAvatar.AvatarAnimationState.Walk:
                animator.SetFloat("Movement_f", 3.5f);
                break;

            case MoveAvatar.AvatarAnimationState.Run:
                animator.SetFloat("Movement_f", 5.5f);
                break;

            case MoveAvatar.AvatarAnimationState.Idle:
                animator.SetFloat("Movement_f", 0f);
                break;
        }
    }

    // Metoda pomocnicza do sprawdzania, czy parametr istnieje
    bool HasParameter(Animator animator, string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
}
