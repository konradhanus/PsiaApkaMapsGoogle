using UnityEngine;

public class ExecutionManager : MonoBehaviour
{
    public static ExecutionManager Instance { get; private set; }

    private bool hasExecuted = false;

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

    public bool HasExecuted()
    {
        return hasExecuted;
    }

    public void SetExecuted(bool executed)
    {
        hasExecuted = executed;
    }
}
