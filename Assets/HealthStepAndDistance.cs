using UnityEngine;
using UnityEngine.UI;
using System;
using BeliefEngine.HealthKit;
using TMPro;
using System.Collections.Generic;

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

    public Text NameDay1ago;
    public Text NameDay2ago;
    public Text NameDay3ago;
    public Text NameDay4ago;
    public Text NameDay5ago;
    public Text NameDay6ago;
    public Text NameDay7ago;
    public TMP_Text resultsLabel;
    private double distanceTotal = 0;

    public TMP_Text TodaySumDistance;
    private bool reading = false;

    void Start()
    {
        

        healthStore = GetComponent<HealthStore>();
        Debug.Log("today2");
        // DISTANCE IS NOW WORKING FIX IT 
        GetDistanceToday();
        // Uruchom sekwencyjny odczyt kroków
        ReadStepsSequentially(1);
    }

    // Metoda rekurencyjna dla sekwencyjnego odczytu kroków
    private void ReadStepsSequentially(int dayOffset)
    {
        // Sprawdź czy nie przekroczyliśmy zakresu dni
        if (dayOffset > 7) return;

        Text dayLabel = GetDayLabel(dayOffset);
        Text dayNameLabel = GetDayNameLabel(dayOffset);

        ReadStepsForSpecificDayAgo(dayOffset, dayLabel, dayNameLabel, () => {
            // Po zakończeniu czytania dla bieżącego dnia, uruchom odczyt dla kolejnego
            ReadStepsSequentially(dayOffset + 1);
        });
    }

    // Funkcja do pobierania odpowiednich referencji do Text dla każdego dnia
    private Text GetDayLabel(int dayOffset)
    {
        switch (dayOffset)
        {
            case 1: return day1ago;
            case 2: return day2ago;
            case 3: return day3ago;
            case 4: return day4ago;
            case 5: return day5ago;
            case 6: return day6ago;
            case 7: return day7ago;
            default: return null;
        }
    }

    private Text GetDayNameLabel(int dayOffset)
    {
        switch (dayOffset)
        {
            case 1: return NameDay1ago;
            case 2: return NameDay2ago;
            case 3: return NameDay3ago;
            case 4: return NameDay4ago;
            case 5: return NameDay5ago;
            case 6: return NameDay6ago;
            case 7: return NameDay7ago;
            default: return null;
        }
    }

    public void TotalStepCounter(string joinDate) {

        Debug.Log("JOIN DATE" + joinDate);
        ReadStepSinceJoin(joinDate, resultsLabel);
    }

    // Funkcja do czytania kroków dla konkretnego dnia wstecz
    public void ReadStepsForSpecificDayAgo(int daysAgo, Text label, Text dayNameLabel, Action onComplete)
    {
        if (reading) return;

        reading = true;

        DateTimeOffset end = DateTimeOffset.UtcNow.AddDays(-daysAgo).Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(-daysAgo).Date;

        dayNameLabel.text = start.ToString("dddd").ToUpper() + " \n" + start.ToString("MM-dd");

        healthStore.ReadSteps(start, end, (double steps, Error error) =>
        {
            if (error != null)
            {
                Debug.LogError("Błąd przy odczycie kroków: " + error.localizedDescription);
                label.text = "Error";
            }
            else if (steps > 0)
            {
                label.text = steps.ToString();
            }
            else
            {
                label.text = "0";
            }
            
            reading = false;
            onComplete?.Invoke(); // Wywołaj callback po zakończeniu
        });
    }

    // Funkcja do czytania kroków dla konkretnego dnia wstecz
    public void ReadStepSinceJoin(string joinDate, TMP_Text label)
    {

        DateTimeOffset end = DateTimeOffset.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        DateTime start = DateTime.ParseExact(joinDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        
        healthStore.ReadSteps(start, end, (double steps, Error error) =>
        {
            if (error != null)
            {
                Debug.LogError("Błąd przy odczycie kroków: " + error.localizedDescription);
                label.text = "Error";
            }
            else if (steps > 0)
            {
                label.text = steps.ToString();
            }
            else
            {
                label.text = "0";
            }

            
        });
    }

    public void GetDistanceToday()
    {
        Debug.Log("today2 in");

        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset startOfDay = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero); // Początek dzisiejszego dnia
        DateTimeOffset endOfDay = startOfDay.AddDays(1); // Końcowy czas to początek następnego dnia


        this.healthStore.ReadQuantitySamples(HKDataType.HKQuantityTypeIdentifierDistanceWalkingRunning, startOfDay, endOfDay, delegate (List<QuantitySample> samples, Error error) {
            if (error != null)
            {
                Debug.LogError("Error fetching distance: " + error.localizedDescription);
                return;
            }

            // Zresetuj distanceTotal na początku
            distanceTotal = 0;

            foreach (QuantitySample sample in samples)
            {
                double sampleDistance = sample.quantity.doubleValue; // Pobierz wartość dystansu z próbki
                distanceTotal += sampleDistance; // Dodaj dystans do distanceTotal
                Debug.Log(string.Format("DISTANCEX - {0} from {1} to {2}", sampleDistance, sample.startDate, sample.endDate));
            }

            // Loguj całkowity dystans po przetworzeniu wszystkich próbek
            Debug.Log($"Total distance for today: {distanceTotal} miles");
            TodaySumDistance.text = distanceTotal.ToString() + " km";
        });
    }

}
