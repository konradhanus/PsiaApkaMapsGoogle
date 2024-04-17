using UnityEngine;

public class AvatarSetter : MonoBehaviour
{
    public Avatar manAvatar;
    public Avatar womanAvatar;
    public GameObject manGameObject;
    public GameObject womanGameObject;
    public Gender defaultGender = Gender.Man;

    private Animator animator;

    public enum Gender
    {
        Woman,
        Man
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        SetAvatar(defaultGender); // Domyślnie ustawiamy ManAvatar
    }

    public void SetAvatar(Gender gender)
    {
        switch (gender)
        {
            case Gender.Woman:
                animator.avatar = womanAvatar;
                womanGameObject.SetActive(true);
                manGameObject.SetActive(false);
                break;
            case Gender.Man:
                animator.avatar = manAvatar;
                womanGameObject.SetActive(false);
                manGameObject.SetActive(true);
                break;
            default:
                Debug.LogError("Nieprawidłowa wartość płci.");
                break;
        }
    }

    public void SetManGender()
    {
        SetAvatar(Gender.Man);
        defaultGender = Gender.Man;
    }

    public void SetWomanGender()
    {
        SetAvatar(Gender.Woman);
        defaultGender = Gender.Woman;
    }
}
