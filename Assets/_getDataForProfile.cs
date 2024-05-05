using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json; // Dodaj referencję do biblioteki Newtonsoft.Json

public class DogSpotCounter : MonoBehaviour
{
    public TextMeshProUGUI counterText; 

    private string baseUrl = "https://psiaapka.pl/getmydogspot.php?user_id=";
    private List<string> dogSpotIds = new List<string>();
    public string userId;
    private FirebaseAuthManager authManager;

    private void Start()
    {
        authManager = new FirebaseAuthManager();
        // Wykonaj call do API i zaktualizuj dane
        userId = ReferencesUserFirebase.userId;
        print("Player Id:" + userId);
        StartCoroutine(GetDogSpots());
    }

    IEnumerator GetDogSpots()
    {
        string url = baseUrl + userId;
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Błąd pobierania danych: " + www.error);
            yield break;
        }

        string jsonString = www.downloadHandler.text;
        
        Debug.Log("STRING" + jsonString);
        // Parsowanie danych JSON z użyciem Newtonsoft.Json
        DogSpotData[] dogSpotDataArray = JsonConvert.DeserializeObject<DogSpotData[]>(jsonString);
        
        // Wczytanie danych o odwiedzonych dogspotach
        foreach (DogSpotData data in dogSpotDataArray)
        {
            dogSpotIds.Add(data.dog_spot_id);
        }

        // Aktualizacja licznika i pola TextMeshPro
        int dogSpotCount = dogSpotIds.Count;
        Debug.Log("Liczba odwiedzonych dogspotów: " + dogSpotCount);
        counterText.text = ""+dogSpotCount;
    }

    // Struktura danych do parsowania JSON
    [System.Serializable]
    public class DogSpotData
    {
        public string id;
        public string dog_spot_id;
        public string user_id;
        public string visit_date;
        public string message;
    }
}
