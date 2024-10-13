using UnityEngine;
using UnityEngine.UI;
using System;
using BeliefEngine.HealthKit;
using TMPro;
using System.Collections.Generic;
using Score;
using System.Collections;
using UnityEngine.Networking;

public class HealthStepAndDistance : MonoBehaviour
{
    private HealthStore healthStore;

    public string userId = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    private FirebaseAuthManager authManager;


    public GameObject Clepsidra; 
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

    
    public GameObject CircleFood;
    public GameObject CircleFun;
    public GameObject CircleStep;
    public TMP_Text StepValueToday;
    public GameObject CircleWater;

    private double distanceTotal = 0;
    private double distanceSinceJoinTotal = 0;

    public TMP_Text levelNumber;
    public Slider slider;
    public TMP_Text score;


    public TMP_Text TodaySumDistance;
    private bool reading = false;

    // Example score to test
    public long playerScore = 2500;

    [System.Serializable]
    public class UserData
    {
        public string nick;
        public int id_avatar;
        public int id_avatar_dog;
        public string date_created;
    }

    [System.Serializable]
    public class DataResponse
    {
        public UserData data;
    }

    UserData userData;




    void Start()
    {
        authManager = new FirebaseAuthManager();
        userId = ReferencesUserFirebase.userId;
        Clepsidra.SetActive(true);
        healthStore = GetComponent<HealthStore>();
        // DISTANCE IS NOW WORKING FIX IT 
        GetDistanceToday();
        // Uruchom sekwencyjny odczyt kroków
        ReadStepsSequentially(1);
    }

    // Metoda rekurencyjna dla sekwencyjnego odczytu kroków
    private void ReadStepsSequentially(int dayOffset)
    {
        // Sprawdź czy nie przekroczyliśmy zakresu dni
        if (dayOffset > 7) {
            FetchData();
            return;
        }
        

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

        Debug.Log("joinDate:" + joinDate);
        ReadStepSinceJoin(joinDate);
    }

    private void UpdateScoreAndLevel(long playerScore)
    {
        // Utworzenie instancji klasy Scoring
        Scoring scoring = new Scoring();

        // Użycie metod klasy Scoring
        int level = scoring.GetCurrentLevel(playerScore);
        float progress = scoring.GetLevelProgress(playerScore);
        string progressText = scoring.GetProgressText(playerScore);

        // Wypisanie wyników do konsoli (lub do interfejsu UI w realnym projekcie Unity)
        Debug.Log($"Poziom gracza: {level}");
        Debug.Log($"Postęp: {progress:P}");
        Debug.Log($"Informacje o postępie: {progressText}");

        // Aktualizacja UI
        score.text = progressText;
        levelNumber.text = level.ToString();
        slider.value = progress;
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

    // Funkcja do czytania kroków z dzisiaj
    public void ReadStepsForToday()
    {
        Debug.Log("pobieram 2");
        Image circle = CircleStep.GetComponent<Image>();
        //if (reading) return;

        //reading = true;

        // Ustalanie początku i końca dnia
        DateTimeOffset start = DateTimeOffset.UtcNow.Date; // Początek dzisiaj (00:00:00)
        DateTimeOffset end = start.AddDays(1).AddSeconds(-1); // Koniec dzisiaj (23:59:59)


        //healthStore.ReadSteps(start, end, (double steps, Error error) =>
        //{
        //    if (error != null)
        //    {
        //        Debug.LogError("Błąd przy odczycie kroków: " + error.localizedDescription);
        //    }
        //    else if (steps > 0)
        //    {
        //        Debug.Log("Circle" + (float)steps / 12000);

        //        Image circle = CircleStep.GetComponent<Image>();
        //        float amount = Mathf.Clamp01((float)steps / 12000);
        //        StepValueToday.text = steps.ToString() ; 
        //        Debug.Log("Circle clamp01" + amount);
        //        circle.fillAmount = amount;
        //    }
        //    else
        //    {
        //        circle.fillAmount = 0f;
        //    }

        //    //reading = false;
        //});

        this.healthStore.ReadQuantitySamples(HKDataType.HKQuantityTypeIdentifierStepCount, start, end, delegate (List<QuantitySample> samples, Error error) {
            if (error != null)
            {
                Debug.LogError("Error fetching distance: " + error.localizedDescription);
                return;
            }

            // Zresetuj distanceTotal na początku
            double stepsTotal = 0;

            foreach (QuantitySample sample in samples)
            {
                double steps = sample.quantity.doubleValue; // Pobierz wartość dystansu z próbki
                stepsTotal += steps; // Dodaj dystans do distanceTotal
                Debug.Log(string.Format("DISTANCEX - {0} from {1} to {2}", stepsTotal, sample.startDate, sample.endDate));
            }

            Debug.Log("Circle" + (float)stepsTotal / 12000);

            Image circle = CircleStep.GetComponent<Image>();
            float amount = Mathf.Clamp01((float)stepsTotal / 12000);
            StepValueToday.text = stepsTotal.ToString();
            Debug.Log("Circle clamp01" + amount);
            circle.fillAmount = amount;
        });

    }

    // Funkcja do czytania kroków dla konkretnego dnia wstecz
    public void ReadStepSinceJoin(string joinDate)
    {

        Debug.Log("ReadStepSinceJoin" + joinDate);
        DateTimeOffset end = DateTimeOffset.UtcNow.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        DateTimeOffset start = DateTimeOffset.ParseExact(joinDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

        this.healthStore.ReadQuantitySamples(HKDataType.HKQuantityTypeIdentifierStepCount, start, end, delegate (List<QuantitySample> samples, Error error) {
            if (error != null)
            {
                Debug.LogError("Error fetching distance: " + error.localizedDescription);
                return;
            }

            // Zresetuj distanceTotal na początku
            double stepsTotalSinceJoin = 0;

            foreach (QuantitySample sample in samples)
            {
                double steps = sample.quantity.doubleValue; // Pobierz wartość dystansu z próbki
                stepsTotalSinceJoin += steps; // Dodaj dystans do distanceTotal
                Debug.Log(string.Format("DISTANCEX - {0} from {1} to {2}", stepsTotalSinceJoin, sample.startDate, sample.endDate));
            }
            UpdateScoreAndLevel((long)stepsTotalSinceJoin);
            Debug.Log("since join krokow: " + stepsTotalSinceJoin);
            TodaySumDistance.text = stepsTotalSinceJoin.ToString() + " km";
            Clepsidra.SetActive(false);
        });
    }

    public void GetDistanceToday()
    {

        Debug.Log("pobieram");

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
            Debug.Log("dzisiaj: "+ distanceTotal);
            TodaySumDistance.text = distanceTotal.ToString() + " km";
        });

        ReadStepsForToday();

    }

    public void GetDistanceSinceJoin(string joinDate)
    {

        Debug.Log("pobieram");
        DateTimeOffset startOfDay = DateTimeOffset.ParseExact(joinDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        DateTimeOffset endOfDay = DateTimeOffset.UtcNow;

        
        this.healthStore.ReadQuantitySamples(HKDataType.HKQuantityTypeIdentifierDistanceWalkingRunning, startOfDay, endOfDay, delegate (List<QuantitySample> samples, Error error) {
            if (error != null)
            {
                Debug.LogError("Error fetching distance: " + error.localizedDescription);
                return;
            }

            // Zresetuj distanceSinceJoinTotal na początku
            distanceSinceJoinTotal = 0;

            foreach (QuantitySample sample in samples)
            {
                double sampleDistance = sample.quantity.doubleValue; // Pobierz wartość dystansu z próbki
                distanceSinceJoinTotal += sampleDistance; // Dodaj dystans do distanceSinceJoinTotal
                Debug.Log(string.Format("DISTANCEX - {0} from {1} to {2}", sampleDistance, sample.startDate, sample.endDate));
            }
            Debug.Log("since join km: " + distanceSinceJoinTotal);
            //TodaySumDistance.text = distanceSinceJoinTotal.ToString() + " km";
        });

        ReadStepsForToday();

    }

    public void FetchData()
    {
        Debug.Log("HealthStepAndDistance: FETCH!, fetch data");
        Debug.Log("HealthStepAndDistance: userId" + userId);
        StartCoroutine(UpdateDataFromAPI());
    }

    IEnumerator UpdateDataFromAPI()
    {
        string url = "https://psiaapka.pl/psiaapka/userData.php?user_id=" + userId;

        Debug.Log("HealthStepAndDistance: FETCH! " + url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("HealthStepAndDistance: Błąd podczas pobierania danych z API: " + request.error);
            yield break;
        }

        string jsonResponse = request.downloadHandler.text;
        DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(jsonResponse);

        if (dataResponse != null && dataResponse.data != null)
        {
            userData = dataResponse.data;
            Debug.Log("HealthStepAndDistance: FETCH!!!" + userData);


            string data_created = dataResponse.data.date_created;
            TotalStepCounter(data_created);
        }
        else
        {
            Debug.LogError("HealthStepAndDistance: Błąd podczas parsowania odpowiedzi z API.");
        }
    }

}
