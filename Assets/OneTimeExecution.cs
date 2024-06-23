using UnityEngine;

public class OneTimeExecution : MonoBehaviour
{
    void Start()
    {
        if (!ExecutionManager.Instance.HasExecuted())
        {
            // Twój kod, który ma się wykonać tylko raz
            Debug.Log("AAA Ten kod wykonuje się tylko raz przy uruchomieniu aplikacji.");

            // Ustaw flagę na true, aby zaznaczyć, że kod został już wykonany
            ExecutionManager.Instance.SetExecuted(true);
        }
        else
        {
            Debug.Log("AAA Ten kod został już wcześniej wykonany.");
            gameObject.SetActive(false);
        }
    }
}
