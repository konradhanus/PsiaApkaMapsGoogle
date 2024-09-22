using UnityEngine;

public class Coonsole
{
    private string prefix;
    private bool loggingEnabled;

    public Coonsole(string prefix, bool loggingEnabled = true)
    {
        this.prefix = prefix;
        this.loggingEnabled = loggingEnabled;
    }

    public void Log(object message)
    {
        if (loggingEnabled)
        {
            Debug.Log($"[{prefix}] {message}");
        }
    }

    public void Warn(object message)
    {
        if (loggingEnabled)
        {
            Debug.LogWarning($"[{prefix}] {message}");
        }
    }

    public void Error(object message)
    {
        if (loggingEnabled)
        {
            Debug.LogError($"[{prefix}] {message}");
        }
    }

    public void SetLoggingEnabled(bool isEnabled)
    {
        loggingEnabled = isEnabled;
    }
}
