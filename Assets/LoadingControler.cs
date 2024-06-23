using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingControler : MonoBehaviour
{
    void Start()
    {
        // Ustawienie początkowej wartości
        LoadingScreenStore.isVisible = true;
        Debug.Log("XXX Scene1: Initial score set to " + LoadingScreenStore.isVisible);
    }
}
