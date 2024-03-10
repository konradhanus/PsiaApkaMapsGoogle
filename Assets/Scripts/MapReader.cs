using System;                       // podstawowe klasy i struktury języka C#
using System.Collections;           // interfejsy i klasy wspomagające kolekcje
using System.Collections.Generic;   // generyczne klasy i interfejsy kolekcji
using System.IO;                    // operacje wejścia/wyjścia (np. pliki)
using System.Xml;                   // umożliwia obsługę operacji na dokumentach XML
using UnityEngine;                  // klasy i funkcje do pracy z silnikiem Unity
using UnityEngine.Networking;       // która obsługuje funkcje sieciowe w Unity
using UnityEngine.UI;               // klasy obsługujące interfejs użytkownika w Unity

class MapReader : MonoBehaviour 
{

    public GameObject popupTextObject;

    float latitude = 51.085235f;
    float longitude = 17.054275f;

    // float latitude = 60.19188000f;
    // float longitude = 24.96858220f;

    [SerializeField]
    double delta = 0.003;

    [SerializeField] public Text GPSStatus;
    [SerializeField] public Text latitudeValue;
    [SerializeField] public Text longitudeValue;
    [SerializeField] public Text altitudeValue;
    [SerializeField] public Text horizontalAccuracyValue;
    [SerializeField] public Text timestampValue;


    private float latitudeGPS;
    private float longitudeGPS;

    // Kolekcja przechowująca węzły mapy OSM
    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    // Lista przechowująca drogi mapy OSM
    [HideInInspector]
    public List<OsmWay> ways;

    // Obiekt przechowujący granice mapy OSM
    [HideInInspector]
    public OsmBounds bounds;

    // Referencja do obiektu reprezentującego powierzchnię ziemi w grze
    public GameObject groundPlane;

    // Właściwość informująca o gotowości do pracy klasy
    public bool IsReady { get; private set; } 

    [SerializeField]
    public bool debugMode = true;

    // Metoda wywoływana przy starcie aplikacji
    void Start()
    {
        StartCoroutine(InitializeGPSAndLoadOSMData());
    }

    IEnumerator InitializeGPSAndLoadOSMData()
    {
        if(!debugMode) 
        {
            // Sprawdzenie, czy urządzenie obsługuje GPS
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("GPS jest wyłączony. Włącz go, aby korzystać z tej funkcji.");
                    yield break;
            }

            // Inicjalizacja GPS
            Input.location.Start();

            // Czekanie na dostępność danych GPS
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                maxWait--;
                Debug.Log("Czekam na dostępność danych GPS...");
                // Pauza na sekundę
                yield return new WaitForSeconds(1);
            }

            // Sprawdzenie, czy timeout został osiągnięty
            if (maxWait <= 0)
            {
                Debug.Log("Timeout podczas inicjalizacji GPS.");
                yield break;
            }

            // Sprawdzenie, czy GPS jest w stanie dostarczyć dane
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Nie można uzyskać danych GPS.");
                yield break;
            }

            // Jeśli dostępne są dane GPS, pobierz szerokość i długość geograficzną
            latitudeGPS = Input.location.lastData.latitude;
            longitudeGPS = Input.location.lastData.longitude;
        }else{
            latitudeGPS = latitude;
            longitudeGPS = longitude;
        }
        latitudeValue.text = Input.location.lastData.latitude.ToString();
        longitudeValue.text = Input.location.lastData.longitude.ToString();
        altitudeValue.text = Input.location.lastData.altitude.ToString();
        horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
        timestampValue.text = Input.location.lastData.timestamp.ToString();

        // Wyświetlenie danych w konsoli
        Debug.Log("Latitude: " + latitudeGPS + ", Longitude: " + longitudeGPS);

        // Zakończenie usługi GPS
        Input.location.Stop();

        StartCoroutine(DownloadAndLoadOSMData());
    }


    // Asynchroniczna metoda pobierająca i ładowająca dane OSM
    IEnumerator DownloadAndLoadOSMData()
    {
       
        // Poczekaj na zakończenie procesu pobierania danych
        Debug.Log("LATITUDE"+latitudeGPS);
        Debug.Log("LONGITUDE"+longitudeGPS);
        BoundsCalculator.CalculateBounds(longitudeGPS, latitudeGPS, delta, out double left, out double right, out double top, out double bottom);
        Debug.Log("Test");
        Debug.Log("l"+left+"r"+right+"t"+top+"b"+bottom);
                
        string fullUrl = "https://www.openstreetmap.org/api/0.6/map?bbox="+left+"%2C"+bottom+"%2C"+right+"%2C"+top;
        string url = fullUrl.Replace(',', '.');
        Debug.Log(fullUrl);

        Debug.Log("jestem tutaj");
        // Utwórz żądanie sieciowe
        UnityWebRequest www = UnityWebRequest.Get(url);

        // Poczekaj na zakończenie żądania
        yield return www.SendWebRequest(); 

        if (www.isNetworkError || www.isHttpError)
        {
            // Wyświetl popup z informacją o błędzie pobierania danych
            DisplayPopup("Błąd pobierania danych OSM: " + www.error + " URL: " + url);
            Debug.LogError("Błąd pobierania danych OSM: " + www.error+ " URL: " + fullUrl);
        }
        else
        {
            // Otrzymane dane OSM w formie XML
            string osmXMLData = www.downloadHandler.text;

            // Parsowanie XML do XmlDocument
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(osmXMLData);

            // Inicjalizacja słownika węzłów
            nodes = new Dictionary<ulong, OsmNode>();

            // Inicjalizacja listy dróg
            ways = new List<OsmWay>();

            // Inicjalizacja obiektu XmlDocument do analizy XML
            // XmlDocument doc = new XmlDocument();

            // Wczytaj dane XML z pamięci podręcznej
            // doc.LoadXml(cachedData);

            // Ustaw granice mapy na podstawie danych XML
            SetBounds(xmlDoc.SelectSingleNode("/osm/bounds"));

            // Pobierz i przetwórz węzły z danych XML
            GetNodes(xmlDoc.SelectNodes("/osm/node"));

            // Pobierz i przetwórz drogi z danych XML
            GetWays(xmlDoc.SelectNodes("/osm/way")); 

            // Przelicz współrzędne geograficzne na współrzędne w grze
            float minx = (float)MercatorProjection.lonToX(bounds.MinLon);
            float maxx = (float)MercatorProjection.lonToX(bounds.MaxLon);
            float miny = (float)MercatorProjection.latToY(bounds.MinLat);
            float maxy = (float)MercatorProjection.latToY(bounds.MaxLat);

            // Ustaw skalę powierzchni ziemi na podstawie obliczonych współrzędnych
            groundPlane.transform.localScale = new Vector3((maxx - minx) / 2, 1, (maxy - miny) / 2);

            // Oznacz, że klasa jest gotowa do pracy
            IsReady = true; 

            // Wyświetl popup z informacją o pobraniu danych
            if (popupTextObject != null)
            {
                Text popupText = popupTextObject.GetComponent<Text>();
                if (popupText != null)
                {
                    popupText.text = "Dane OSM zostały pomyślnie pobrane!";
                }
            }


            // Wyświetl popup z informacją o zapisaniu danych do pamięci podręcznej
            DisplayPopup("Dane OSM zostały zapisane w pamięci podręcznej.");

            // Tutaj możesz przekazać xmlDoc do swojej klasy MapReader lub innego komponentu, który przetwarza dane OSM.
        }

        
    }

    // Metoda wywoływana co klatkę
    void Update()
    {
        if (bounds != null && nodes != null)
        {
            foreach (OsmWay w in ways)
            {
                if (w.Visible)
                {
                    Color c = Color.cyan; // Ustaw kolor na cyan dla budynków
                    if (!w.IsBoundary) c = Color.red; // Ustaw kolor na czerwony dla dróg

                    for (int i = 1; i < w.NodeIDs.Count; i++)
                    {
                        if (nodes.ContainsKey(w.NodeIDs[i - 1]) && nodes.ContainsKey(w.NodeIDs[i]))
                        {
                            OsmNode p1 = nodes[w.NodeIDs[i - 1]];
                            OsmNode p2 = nodes[w.NodeIDs[i]];

                            Vector3 v1 = p1 - bounds.Centre;
                            Vector3 v2 = p2 - bounds.Centre;

                            Debug.DrawLine(v1, v2, c); // Rysuj linię reprezentującą drogę/budynek w grze
                        }
                        else
                        {
                            Debug.LogWarning("Brak danych dla jednego z węzłów. Sprawdź poprawność danych OSM.");
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Brak danych granic lub węzłów. Sprawdź poprawność danych OSM.");
        }
    }

    // Metoda przetwarzająca drogi z danych XML
    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            // Utwórz obiekt reprezentujący drogę na podstawie danych XML
            OsmWay way = new OsmWay(node);

            // Dodaj drogę do listy
            ways.Add(way); 
        }
    }

    // Metoda przetwarzająca węzły z danych XML
    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            // Utwórz obiekt reprezentujący węzeł na podstawie danych XML
            OsmNode node = new OsmNode(n);

            // Dodaj węzeł do słownika
            nodes[node.ID] = node; 
        }
    }

    // Metoda ustawiająca granice mapy na podstawie danych XML
    void SetBounds(XmlNode xmlNode)
    {
        // dane które ttutaj sa to minlat, minlon, maxlat, maxlon, oraz center
        
        // Utwórz obiekt reprezentujący granice mapy na podstawie danych XML
        bounds = new OsmBounds(xmlNode); 
    }



    // Metoda wyświetlająca komunikat na obiekcie Text
    private void DisplayPopup(string message)
    {
        if (popupTextObject != null)
        {
            Text popupText = popupTextObject.GetComponent<Text>();
            if (popupText != null)
            {
                popupText.text = message;
            }
        }
    }

}
