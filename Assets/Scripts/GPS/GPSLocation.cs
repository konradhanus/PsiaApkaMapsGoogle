using System.Collections;
using UnityEngine;
using UnityEngine.UI;



// Singleton przechowujący dane o lokalizacji
public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }

    public float latitude = 0;
    public float longitude = 0;

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
}

public class GPSLocation : MonoBehaviour
{
    [SerializeField] public Text GPSStatus;
    [SerializeField] public Text latitudeValue;
    [SerializeField] public Text longitudeValue;
    [SerializeField] public Text altitudeValue;
    [SerializeField] public Text horizontalAccuracyValue;
    [SerializeField] public Text timestampValue;
    public Vector3 previousPosition;

    public int iteration = 0;
    private float latitude;
    private float longitude;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
    }

    // Update is called once per frame
    IEnumerator GPSLoc()
    {
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        Input.location.Start();

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            GPSStatus.text = "Time out";
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            GPSStatus.text = "Unable to determine device location";
            yield break;
        }
        else
        {
            GPSStatus.text = "Running";
            // Call the method to update GPS data continuously
            UpdateGPSData();
            // Zapisz poprzednią pozycję
            if (Input.location.status == LocationServiceStatus.Running)
            {
                 Vector3 previousPosition = new Vector3(Input.location.lastData.latitude, 0f, Input.location.lastData.longitude);
            }
        }
        yield return new WaitForSeconds(2);
    }

    void UpdateGPSData()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            iteration++;
            
            
            Vector3 currentGPSPosition = new Vector3(Input.location.lastData.latitude, 0f, Input.location.lastData.longitude);
            Vector3 displacement = currentGPSPosition - previousPosition;

            GPSStatus.text = "Running"+iteration.ToString()+", x:"+displacement.x+", y:"+displacement.y+", z:"+displacement.z;

            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
            LocationManager.Instance.latitude = latitude;
            LocationManager.Instance.longitude = latitude;

            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            timestampValue.text = Input.location.lastData.timestamp.ToString();

        }
        else
        {
            GPSStatus.text = "Stop";
        }
    }

    // Method to get latitude
    public float GetLatitude()
    {
        return latitude;
    }

    // Method to get longitude
    public float GetLongitude()
    {
        return longitude;
    }

     private void Update()
    {
       UpdateGPSData();
    }
}
