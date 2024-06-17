using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    // public void ResetScene()
    // {
    //     // Pobierz nazwę aktualnej sceny
    //     string currentSceneName = SceneManager.GetActiveScene().name;
    //     // Załaduj ponownie tę scenę
    //     SceneManager.LoadScene(currentSceneName);
    // }

    public void BackToMainScreen()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void GoToDogScreen()
    {
        SceneManager.LoadSceneAsync(2);
    }
}
