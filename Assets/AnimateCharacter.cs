using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;

public class AnimateCharacter : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Sprawdzenie, czy Animator istnieje
        if (animator == null)
        {
            Debug.LogError("WARTOSC Animator not found on GameObject!");
            return;
        }

        // Sprawdzenie, czy parametry "speed" i "motion_speed" istniej? w Animatorze
        if (!HasParameter(animator, "Speed"))
        {
            Debug.LogError("WARTOSC Animator parameter 'speed' not found!");
        }

        if (!HasParameter(animator, "MotionSpeed"))
        {
            Debug.LogError("WARTOSC Animator parameter 'motion_speed' not found!");
        }

        // Testowe ustawienie warto?ci
        animator.SetFloat("Speed", 3.5f);
        animator.SetFloat("MotionSpeed", 2f);
        Debug.Log("WARTOSC Animator and parameters checked, setting initial values.");
    }

    public void OnAnimationStateChange(MoveAvatar.AvatarAnimationState animationState)
    {
        Debug.Log("State changed to: " + animationState);

        switch (animationState)
        {
            case MoveAvatar.AvatarAnimationState.Walk:
                animator.SetFloat("Speed", 3.5f);
                animator.SetFloat("MotionSpeed", 2f);
                break;

            case MoveAvatar.AvatarAnimationState.Run:
                animator.SetFloat("Speed", 5.5f);
                animator.SetFloat("MotionSpeed", 2f);
                break;

            case MoveAvatar.AvatarAnimationState.Idle:
                animator.SetFloat("Speed", 0f);
                animator.SetFloat("MotionSpeed", 2f);
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
