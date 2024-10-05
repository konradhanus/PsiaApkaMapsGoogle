using System;
using TMPro;
using UnityEngine;

public class DateHandler : MonoBehaviour
{

    public TextMeshProUGUI dateTextMeshPro;

    // Referencja do skryptu HealthStepAndDistance
    public HealthStepAndDistance healthStepAndDistance;

    void Start()
    {
        // Sprawdź, czy podano wszystkie referencje
        if (dateTextMeshPro != null && healthStepAndDistance != null)
        {
            // Pobierz datę jako string z TextMeshPro
            string dateString = dateTextMeshPro.text;

          Debug.Log("TEST" + dateString);
                // Wywołaj metodę TotalStepCounter z klasy HealthStepAndDistance
                healthStepAndDistance.TotalStepCounter(dateString);  
        }
        else
        {
            Debug.LogError("Brak referencji do GameObject, TextMeshPro lub HealthStepAndDistance.");
        }
    }
}
