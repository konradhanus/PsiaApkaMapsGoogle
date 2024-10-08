using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadingScreenSettings : MonoBehaviour
{
    // Obiekty do kontrolowania
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject profileScreen;
    [SerializeField] private GameObject dogProfileScreen;
    [SerializeField] private GameObject bagScreen;
    [SerializeField] private GameObject backScreen;

    // Checkboxy do decydowania o widoczności w edytorze
    [SerializeField] private bool showLoading= true;
    [SerializeField] private bool showProfile = false;
    [SerializeField] private bool dogProfile = false;
    [SerializeField] private bool bag = false;
    [SerializeField] private bool back = false;

    private void Awake()
    {
        // W trybie gry ekran ładowania jest widoczny, a ekran profilu ukryty
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true); // Zawsze widoczny w grze
        }
        if (profileScreen != null)
        {
            profileScreen.SetActive(false); // Zawsze ukryty w grze
        }
        if (dogProfileScreen != null)
        {
            dogProfileScreen.SetActive(false); // Zawsze ukryty w grze
        }
        if (bagScreen != null)
        {
            bagScreen.SetActive(false); // Zawsze ukryty w grze
        }

        if (backScreen != null)
        {
            backScreen.SetActive(false); // Zawsze ukryty w grze
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // W trybie edycji decydujemy o widoczności ekranów na podstawie checkboxów
        if (!EditorApplication.isPlaying)
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(showLoading);
            }

            if (profileScreen != null)
            {
                profileScreen.SetActive(showProfile);
            }

            if (dogProfileScreen != null)
            {
                dogProfileScreen.SetActive(dogProfile);
            }

            if (bagScreen != null)
            {
                bagScreen.SetActive(bag);
            }

            if (backScreen != null)
            {
                backScreen.SetActive(back);
            }

        }
    }
#endif
}
