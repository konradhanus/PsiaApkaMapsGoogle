using UnityEngine;
using System.Collections;

public class ScaleChange : MonoBehaviour
{
    public GameObject targetObject; // Publiczne pole przechowujące obiekt, którego skalę chcemy zmieniać

    private Vector3 initialScale;
    private float scaleChangeSpeed = 1f; // Prędkość zmiany skali
    private float delayBetweenChanges = 3f; // Opóźnienie między zmianami
    private bool scalingUp = true; // Flaga określająca, czy obiekt powinien powiększać się czy zmniejszać

    private void Start()
    {
        if(targetObject == null)
        {
            Debug.LogError("Target object is not assigned!");
            enabled = false; // Wyłącz skrypt, jeśli obiekt docelowy nie został przypisany
            return;
        }

        initialScale = targetObject.transform.localScale;
        InvokeRepeating("ChangeScale", 0f, delayBetweenChanges);
    }

    private void ChangeScale()
    {
        Debug.Log("ChangeScale() called");
        
        if (scalingUp)
        {
            Debug.Log("Scaling up...");
            StartCoroutine(ScaleOverTime(initialScale, Vector3.one, 7f));
        }
        else
        {
            Debug.Log("Scaling down...");
            StartCoroutine(ScaleOverTime(Vector3.one, initialScale, 7f));
        }
        scalingUp = !scalingUp; // Zmiana kierunku zmiany skali
    }

    private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 endScale, float duration)
    {
        Debug.Log("ScaleOverTime() called");

        float startTime = Time.time;
        float endTime = startTime + duration;
        while (Time.time < endTime)
        {
            float normalizedTime = (Time.time - startTime) / duration;
            float smoothTime = Mathf.SmoothStep(0f, 1f, normalizedTime); // Wykorzystaj funkcję SmoothStep do płynnej interpolacji
            targetObject.transform.localScale = Vector3.Lerp(startScale, endScale, smoothTime);
            yield return null;
        }
        targetObject.transform.localScale = endScale;
        Debug.Log("ScaleOverTime() completed");
    }
}
