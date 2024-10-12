using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneLoader : MonoBehaviour
{
    public void LoadScene6()
    {
        SceneManager.LoadScene(6); // Możesz też użyć nazwy sceny, np. SceneManager.LoadScene("SceneName");
    }

    public void LoadScene1()
    {
        SceneManager.LoadScene(1); // Możesz też użyć nazwy sceny, np. SceneManager.LoadScene("SceneName");
    }
}
