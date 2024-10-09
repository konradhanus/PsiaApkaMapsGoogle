using System;
using TMPro;
using UnityEngine;

public class DateHandler : MonoBehaviour
{
    public TextMeshProUGUI dateTextMeshPro;

    // Referencja do skryptu HealthStepAndDistance
    public HealthStepAndDistance healthStepAndDistance;

    private float timer = 0f;  // Licznik czasu

    void Start()
    {
        // Sprawdź, czy podano wszystkie referencje
        if (dateTextMeshPro != null && healthStepAndDistance != null)
        {
            // Pobierz datę jako string z TextMeshPro
            string dateString = dateTextMeshPro.text;

            // Wywołaj metodę TotalStepCounter z klasy HealthStepAndDistance
            healthStepAndDistance.TotalStepCounter(dateString);

            // Rozpocznij wywoływanie GetAllStepsSinceJoin co 0.5 sekundy
            InvokeRepeating("GetAllStepsSinceJoin", 0.5f, 0.5f);
        }
        else
        {
            Debug.LogError("Brak referencji do GameObject, TextMeshPro lub HealthStepAndDistance.");
        }
    }

    void Update()
    {
        // Zwiększ licznik czasu o czas, który upłynął od ostatniej ramki
        timer += Time.deltaTime;

        // Sprawdź, czy minęło 3 sekund
        if (timer >= 3f)
        {
            // Zatrzymaj wywoływanie GetAllStepsSinceJoin
            CancelInvoke("GetAllStepsSinceJoin");
        }
    }

    public void GetAllStepsSinceJoin()
    {
        // Sprawdź, czy podano wszystkie referencje
        if (dateTextMeshPro != null && healthStepAndDistance != null)
        {
            // Pobierz datę jako string z TextMeshPro
            string dateString = dateTextMeshPro.text;

            // Wywołaj metodę TotalStepCounter z klasy HealthStepAndDistance
            healthStepAndDistance.TotalStepCounter(dateString);
        }
        else
        {
            Debug.LogError("Brak referencji do GameObject, TextMeshPro lub HealthStepAndDistance.");
        }
    }
}
