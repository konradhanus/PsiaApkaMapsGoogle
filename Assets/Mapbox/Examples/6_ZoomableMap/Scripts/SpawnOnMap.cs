namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine.Networking;
	using Newtonsoft.Json;
	using UnityEngine.UI;               // klasy obsługujące interfejs użytkownika w Unity	

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

 		[SerializeField] public Text dogSpotStatus;
		List<GameObject> _spawnedObjects;

		List<DataObject> dataObjects;

		 [SerializeField]
    	public bool debugMode = true;

		// Wartości latitude, longitude i promienia
		float latitude = 51.093114f;
		float longitude = 17.035030f;
		float radius = 0.5f;

		private float latitudeGPS;
    	private float longitudeGPS;
		
		// Adres URL endpointu
		string endpointURL = "https://psiaapka.pl/psiaapka/dogspots.php";

		void Start()
		{
			_spawnedObjects = new List<GameObject>();
			StartCoroutine(FetchData());

			// _locations = new Vector2d[_locationStrings.Length];
			// Debug.Log(_locations);
			
			// for (int i = 0; i < _locationStrings.Length; i++)
			// {
			// 	var locationString = _locationStrings[i];
			// 	_locations[i] = Conversions.StringToLatLon(locationString);
			// 	var instance = Instantiate(_markerPrefab);
			// 	instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
			// 	instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			// 	_spawnedObjects.Add(instance);
			// }
		}

		IEnumerator FetchData()
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
			// Tworzenie zapytania URL
			string url = $"{endpointURL}?latitude={latitudeGPS.ToString().Replace(",", ".")}&longitude={longitudeGPS.ToString().Replace(",", ".")}&radius={radius.ToString().Replace(",", ".")}";

			Debug.Log("FETCH DATA");
			Debug.Log(url);
			// Wysłanie zapytania do serwera
			UnityWebRequest request = UnityWebRequest.Get(url);
			yield return request.SendWebRequest();
			Debug.Log("poszło zapytanie");
			dogSpotStatus.text = "poszło zapytanie";
			// Sprawdzenie czy wystąpił błąd
			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError(request.error);
				yield break;
			}
			Debug.Log(request.result);
			Debug.Log("Sukcess");
			dogSpotStatus.text = "Sukcess";
			 // Pobranie odpowiedzi w formie JSON
			string jsonResponse = request.downloadHandler.text;
			Debug.Log("Jest json" + jsonResponse);
			dogSpotStatus.text = "Jest json" + jsonResponse;
			// Przetworzenie JSON na listę obiektów
			dataObjects = JsonConvert.DeserializeObject<List<DataObject>>(jsonResponse); // Użyj JsonConvert.DeserializeObject
			Debug.Log("Data objects");
			dogSpotStatus.text = "Data objects";
			Debug.Log("Data objects");
			// Debug.Log(dataObjects);
			// dogSpotStatus.text = dataObjects;
			_locations = new Vector2d[dataObjects.Count];
			Debug.Log(_locations);
			dogSpotStatus.text = "_locations";
			// _spawnedObjects = new List<GameObject>();
			// Przykładowe wykorzystanie pobranych danych
			int i = 0;
			foreach (var dataObject in dataObjects)
			{
				dogSpotStatus.text = "foreach";
				Debug.Log($"ID: {dataObject.id}, Latitude: {dataObject.latitude}, Longitude: {dataObject.longitude}, Type: {dataObject.type}, Name: {dataObject.name}");
				
				_locations[i] = Conversions.StringToLatLon($"{dataObject.latitude.ToString().Replace(",", ".")}, {dataObject.longitude.ToString().Replace(",", ".")}");

				dogSpotStatus.text = "_locations[i]";
				
				var instance = Instantiate(_markerPrefab);
				
				dogSpotStatus.text = "instance";
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				dogSpotStatus.text = "localPosition";
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				dogSpotStatus.text = "new Vector3(_spawnScale, _spawnScale, _spawnScale)";
				_spawnedObjects.Add(instance);
				dogSpotStatus.text = "_spawnedObjects.Add(instance);";
				i++;
				dogSpotStatus.text = i.ToString();
			}

		}
	

		public static class JsonHelper
		{
			// Metoda do konwersji JSON do tablicy obiektów
			public static T[] FromJson<T>(string json)
			{
				Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
				return wrapper.Items;
			}

			// Klasa pomocnicza do obsługi JSON
			[System.Serializable]
			private class Wrapper<T>
			{
				public T[] Items;
			}
		}

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}
	}


	[System.Serializable]
	public class DataObject
	{
		public string id;
		public float latitude;
		public float longitude;
		public string type;
		public string name;
	}
}