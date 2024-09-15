using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using GoShared;

namespace GoMap
{
    public class GoPlaces2 : MonoBehaviour
    {
        public GOMap goMap;
        public GameObject prefab;
        private GameObject placedObject;

        // Warto?ci latitude, longitude i promienia
        float latitude = 51.08666657545862f;
        float longitude = 17.05298076897365f;
        float radius = 0.5f;

        // Adres URL endpointu
        public string endpointURL = "https://psiaapka.pl/psiaapka/dogspots.php";

        List<DataObject> dataObjects;

        void Awake()
        {
            goMap.OnTileLoad.AddListener((GOTile) => {
                OnLoadTile(GOTile);
            });
        }

        void OnLoadTile(GOTile tile)
        {
            // Only place objects if they haven't been placed yet
            if (placedObject == null)
            {
                StartCoroutine(FetchData(tile));
            }
        }

        IEnumerator FetchData(GOTile tile)
        {
            // Tworzenie zapytania URL
            string url = $"{endpointURL}?latitude={latitude}&longitude={longitude}&radius={radius}";
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }

            // Pobranie odpowiedzi w formie JSON
            string jsonResponse = request.downloadHandler.text;
            dataObjects = JsonConvert.DeserializeObject<List<DataObject>>(jsonResponse);

            foreach (var dataObject in dataObjects)
            {
                // Stw?rz obiekt na podstawie danych
                PlaceObjectOnMap(tile, dataObject);
            }
        }

        void PlaceObjectOnMap(GOTile tile, DataObject dataObject)
        {
            // Konwersja wsp??rz?dnych z obiektu dataObject
            Coordinates coordinates = new Coordinates(dataObject.latitude, dataObject.longitude, 0);

            // Sprawd?, czy dane wsp??rz?dne nale?? do aktualnie wczytywanego kafla
            if (TileFilter(tile, coordinates))
            {
                // Instantiate prefab
                placedObject = Instantiate(prefab);
                placedObject.SetActive(true);

                // Convert coordinates to position
                Vector3 position = coordinates.convertCoordinateToVector(placedObject.transform.position.y);

                if (goMap.useElevation)
                    position = GOMap.AltitudeToPoint(position);

                // Set the position of the object
                placedObject.transform.localPosition = position;
                placedObject.transform.SetParent(tile.transform);
                placedObject.name = dataObject.name;
            }
        }

        bool TileFilter(GOTile tile, Coordinates coordinates)
        {
            Vector2 tileCoordinates = coordinates.tileCoordinates(goMap.zoomLevel);
            return tile.goTile.tileCoordinates.Equals(tileCoordinates);
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
}
