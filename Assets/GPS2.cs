using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GPS2 : MonoBehaviour
{
    [SerializeField] public Text GPSStatus; 
    [SerializeField] public Text latitudeValue;
    [SerializeField] public Text longitudeValue;
    [SerializeField] public Text altitudeValue;
    [SerializeField] public Text horizontalAccuracyValue;
    [SerializeField] public Text timestampValue;

    private bool gpsEnabled;
    private float updateInterval = 2f; // Czas odświeżania w sekundach

    void Start()
    {
        // Sprawdzanie, czy GPS jest włączony
        gpsEnabled = Input.location.isEnabledByUser;

        // Jeśli GPS jest wyłączony, wyświetl odpowiedni komunikat
        if (!gpsEnabled)
        {
            GPSStatus.text = "GPS is not enabled.";
            return;
        }

        // Start GPS
        Input.location.Start();
        GPSStatus.text = "Initializing GPS...";

        // Czekaj na aktywację GPS
        StartCoroutine(InitializeGPS());
    }

    IEnumerator InitializeGPS()
    {
        // Czekaj, aż GPS zostanie zainicjowany
        while (Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(1);
        }

        // Jeśli GPS nie został zainicjowany poprawnie, wyświetl odpowiedni komunikat
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Failed to initialize GPS.";
            yield break;
        }

        GPSStatus.text = "GPS initialized.";

        // Aktualizuj pozycję co określony czas
        InvokeRepeating("UpdateGPSData", 0f, updateInterval);
    }

    void UpdateGPSData()
    {
        // Pobierz aktualną pozycję GPS
        LocationInfo location = Input.location.lastData;

        // Aktualizuj pola tekstowe z wartościami GPS
        latitudeValue.text = "Latitude: " + location.latitude.ToString();
        longitudeValue.text = "Longitude: " + location.longitude.ToString();
        altitudeValue.text = "Altitude: " + location.altitude.ToString();
        horizontalAccuracyValue.text = "Horizontal Accuracy: " + location.horizontalAccuracy.ToString();
        timestampValue.text = "Timestamp: " + location.timestamp.ToString();
    }

    void OnDestroy()
    {
        // Zatrzymaj GPS przy zamykaniu aplikacji
        Input.location.Stop();
    }
}
