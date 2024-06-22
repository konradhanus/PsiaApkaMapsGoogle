using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(int sceneName)
    {
        // Zapamiętaj aktualną scenę przed przejściem do nowej
        if (SceneTracker.Instance != null)
        {
            SceneTracker.Instance.SetPreviousScene(SceneManager.GetActiveScene().name);
        }
        SceneManager.LoadSceneAsync(sceneName);
    }
}