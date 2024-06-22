using UnityEngine;

public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance { get; private set; }

    public string PreviousScene { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPreviousScene(string sceneName)
    {
        PreviousScene = sceneName;
    }
}