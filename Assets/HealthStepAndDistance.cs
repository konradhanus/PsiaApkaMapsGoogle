using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using BeliefEngine.HealthKit;
using TMPro;
using System.Collections;

public class HealthStepAndDistance : MonoBehaviour
{

    private HealthStore healthStore;

    public Text day1ago;
    public Text day2ago;
    public Text day3ago;
    public Text day4ago;
    public Text day5ago;
    public Text day6ago;
    public Text day7ago;
    public Text resultsLabel;

    public Text Sum;
    private bool reading = false;


    void Start()
    {
        this.healthStore = this.GetComponent<HealthStore>();
        // Uruchom funkcje sekwencyjnie
        StartCoroutine(ReadAllDaysSequentially());
        Debug.Log("CO TO JEST START");
    }

    IEnumerator ReadAllDaysSequentially()
    {
        Debug.Log("CO TO JEST iterator");
        yield return ReadStepsForSpecificDayAgo(1, day1ago);
        Debug.Log("CO TO JEST iterator1");
        yield return ReadStepsForSpecificDayAgo(2, day2ago);
        Debug.Log("CO TO JEST iterator2");
        yield return ReadStepsForSpecificDayAgo(3, day3ago);
        yield return ReadStepsForSpecificDayAgo(4, day4ago);
        yield return ReadStepsForSpecificDayAgo(5, day5ago);
        yield return ReadStepsForSpecificDayAgo(6, day6ago);
        yield return ReadStepsForSpecificDayAgo(7, day7ago);
    }

    public void ReadSteps()
    {
        //DateTimeOffset end = DateTimeOffset.UtcNow;
        //DateTimeOffset start = end.AddDays(-1);

        // Ustawiamy datę końcową na wczoraj o 23:59:59
        DateTimeOffset end = DateTimeOffset.UtcNow.AddDays(-1).Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        // Ustawiamy datę początkową na wczoraj o 00:00:00
        DateTimeOffset start = end.Date;

        this.healthStore.ReadSteps(start, end, delegate (double steps, Error error)
        {
            if (steps > 0)
            {
                this.resultsLabel.text += "total steps:" + steps + ": " + end.ToString() + " - " + start.ToString();
            }
            else
            {
                this.resultsLabel.text += "No steps during this period." + end.ToString() + " - " + start.ToString();
            }

            // all done
            reading = false;
        });
    }

    // 1. Metoda liczy kroki od podanej daty (od początku tego dnia) do końca dzisiejszego dnia (23:59:59)
    public void ReadStepsFromDateToNow(DateTimeOffset startDate)
    {
        // Ustawiamy start na początek podanego dnia (00:00:00)
        DateTimeOffset start = startDate.Date;
        // Ustawiamy koniec na koniec dzisiejszego dnia (23:59:59)
        DateTimeOffset end = DateTimeOffset.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        // Wyświetlamy zakresy dat w logach z nową linią
        Debug.Log($"Reading steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n");

        this.healthStore.ReadSteps(start, end, delegate (double steps, Error error)
        {
            if (steps > 0)
            {
                this.resultsLabel.text += $"Total steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}: {steps}\n";
            }
            else
            {
                this.resultsLabel.text += $"No steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n";
            }
            reading = false;
        });
    }

    // 2. Metoda liczy kroki z ostatniego tygodnia (każdy dzień od 00:00:00 do 23:59:59)
    public void ReadStepsFromLastWeek()
    {
        // Ustawiamy start na początek dnia, 7 dni temu (00:00:00)
        DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(-7).Date;
        // Ustawiamy koniec na koniec dzisiejszego dnia (23:59:59)
        DateTimeOffset end = DateTimeOffset.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        // Wyświetlamy zakresy dat w logach z nową linią
        Debug.Log($"Reading steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n");

        this.healthStore.ReadSteps(start, end, delegate (double steps, Error error)
        {
            if (steps > 0)
            {
                this.resultsLabel.text += $"Total steps from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}: {steps}\n";
            }
            else
            {
                this.resultsLabel.text += $"No steps in the last week from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n";
            }
            reading = false;
        });
    }

    // 3. Metoda liczy kroki dla konkretnego dnia wstecz (od 00:00:00 do 23:59:59)
    public IEnumerator ReadStepsForSpecificDayAgo(int daysAgo, Text label)
    {
        if (!reading)
        {
            reading = true; // Rozpocznij odczyt

            yield return StartCoroutine(ReadStepsCoroutine(daysAgo, label));

            reading = false; // Zakończ odczyt
        }
    }

    private IEnumerator ReadStepsCoroutine(int daysAgo, Text label)
    {

        HealthStore healthStore2 = this.GetComponent<HealthStore>();
        Debug.Log("CO TO JEST in1");

        DateTimeOffset end = DateTimeOffset.UtcNow.AddDays(-daysAgo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(-daysAgo).Date;
        Debug.Log("CO TO JEST in3");

        Debug.Log($"Reading steps for {daysAgo} days ago: from {start.ToString("yyyy-MM-dd HH:mm:ss")} to {end.ToString("yyyy-MM-dd HH:mm:ss")}\n");

        bool hasError = false; // Zmienna do monitorowania błędów
        Debug.Log("CO TO JEST in4");
        // Użyj zagnieżdżonego delegata do obsługi odczytu kroków
        healthStore2.ReadSteps(start, end, delegate (double steps, Error error)
        {
            Debug.Log("CO TO JEST in5");
            try
            {
                Debug.Log("CO TO JEST in6");
                if (error != null)
                {
                    Debug.Log("CO TO JEST in7");
                    Debug.LogError("Błąd przy odczycie kroków: " + error.localizedDescription);
                    label.text = "Error";
                    hasError = true; // Ustaw flagę błędu
                }
                else if (steps > 0)
                {
                    Debug.Log("CO TO JEST in8");
                    label.text = steps.ToString();
                    Debug.Log("CO TO JEST in TEXT:" + steps.ToString());
                }
                else
                {
                    Debug.Log("CO TO JEST in9");
                    label.text = "0";
                }
            }
            catch (Exception ex)
            {
                Debug.Log("CO TO JEST in10: " + ex.Message);
                Debug.LogError("Wystąpił wyjątek podczas przetwarzania kroków: " + ex.Message);
                label.text = "Error";
                hasError = true; // Ustaw flagę błędu
            }
            finally
            {
                Debug.Log("CO TO JEST in11: ");
                reading = false; // Zakończ odczyt
            }
        });

        // Poczekaj, aż zakończy się odczytywanie
        while (reading)
        {
            yield return null;
        }

        // Sprawdź, czy wystąpił błąd
        if (hasError)
        {
            Debug.LogError("Wystąpił błąd w odczycie kroków.");
        }
    }





    public void Wczoraj()
    {

        ReadStepsForSpecificDayAgo(1, day1ago);
    }

    public void Przedwczoraj()
    {

        ReadStepsForSpecificDayAgo(2, day2ago);
    }

    public void TrzyDniTemu()
    {

        ReadStepsForSpecificDayAgo(3, day3ago);
    }

    public void CzteryDniTemu()
    {

        ReadStepsForSpecificDayAgo(4, day4ago);
    }

    public void PiecDniTemu()
    {

        ReadStepsForSpecificDayAgo(5, day5ago);
    }

    public void SzescDniTemu()
    {

        ReadStepsForSpecificDayAgo(6, day6ago);
    }

    public void SiedemDniTemu()
    {

        ReadStepsForSpecificDayAgo(7, day7ago);
    }
}
