using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    public void NavigateToScene0()
    {
        SceneManager.LoadScene(0);
    }
     public void NavigateToScene1()
    {
        SceneManager.LoadScene(1);
    }
}