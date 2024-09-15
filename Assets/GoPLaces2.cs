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
        private List<GameObject> placedObjects = new List<GameObject>();

        // Warto?ci latitude, longitude i promienia
        float latitude = 51.08666657545862f;
        float longitude = 17.05298076897365f;
        float radius = 0.5f;

        // Adres URL endpointu
        public string endpointURL = "https://psiaapka.pl/psiaapka/dogspots_v2.php";

        List<DataObject> dataObjects;

        // Parametr debugowania
        public bool debug = true; // W??czony debug

        void Awake()
        {
            if (debug) Debug.Log("GO_PLACES2: Awake - Initializing.");
            goMap.OnTileLoad.AddListener((GOTile tile) => {
                if (debug) Debug.Log($"GO_PLACES2: OnTileLoad - Tile loaded: {tile}");
                StartCoroutine(OnLoadTile(tile));
            });
        }

        IEnumerator OnLoadTile(GOTile tile)
        {
            if (debug)
            {
                Debug.Log($"GO_PLACES2: OnLoadTile - Starting for tile: {tile}");
                Debug.Log($"GO_PLACES2: Map Tile Coordinates (Lat, Lon): {tile.goTile.tileCenter.latitude}, {tile.goTile.tileCenter.longitude}");
            }
            yield return StartCoroutine(FetchData(tile));

            if (debug) Debug.Log("GO_PLACES2: OnLoadTile - Data fetched, placing objects.");

            foreach (var dataObject in dataObjects)
            {
                PlaceObjectOnMap(tile, dataObject);
            }
        }

        IEnumerator FetchData(GOTile tile)
        {
            if (debug) Debug.Log("GO_PLACES2: FetchData - Starting data fetch.");

            // Wy?wietlenie wsp??rz?dnych uderzaj?cych do endpointa
            string latitudeStr = tile.goTile.tileCenter.latitude.ToString().Replace(',', '.');
            string longitudeStr = tile.goTile.tileCenter.longitude.ToString().Replace(',', '.');
            string radiusStr = radius.ToString().Replace(',', '.');

            // Tworzenie URL z poprawnym formatowaniem
            string url = $"{endpointURL}?latitude={latitudeStr}&longitude={longitudeStr}&radius={radiusStr}";
            if (debug) Debug.Log($"GO_PLACES2: FetchData - Fetching from URL: {url}");

            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                yield break;
            }

            string jsonResponse = request.downloadHandler.text;
            if (debug) Debug.Log($"GO_PLACES2: FetchData - Data fetched: {jsonResponse.Length} characters.");

            dataObjects = JsonConvert.DeserializeObject<List<DataObject>>(jsonResponse);
            if (debug) Debug.Log($"GO_PLACES2: FetchData - Parsed {dataObjects.Count} objects.");
        }

        void PlaceObjectOnMap(GOTile tile, DataObject dataObject)
        {
            if (debug) Debug.Log($"GO_PLACES2: PlaceObjectOnMap - Placing object: {dataObject.name}");

            Coordinates coordinates = new Coordinates(dataObject.latitude, dataObject.longitude, 0);

            if (TileFilter(tile, coordinates))
            {
                GameObject newObject = Instantiate(prefab);
                newObject.SetActive(true);

                Vector3 position = coordinates.convertCoordinateToVector(newObject.transform.position.y);

                if (goMap.useElevation)
                    position = GOMap.AltitudeToPoint(position);

                newObject.transform.localPosition = position;
                newObject.transform.SetParent(tile.transform);
                newObject.name = dataObject.name;

                if (debug)
                {
                    Debug.Log($"GO_PLACES2: PlaceObjectOnMap - Object {dataObject.name} placed at {position}.");
                    Debug.Log($"GO_PLACES2: PlaceObjectOnMap - DataObject Coordinates (Lat, Lon): {dataObject.latitude}, {dataObject.longitude}");
                }

                placedObjects.Add(newObject);
            }
            else
            {
                if (debug) Debug.Log($"GO_PLACES2: PlaceObjectOnMap - Object {dataObject.name} not placed, outside tile.");
            }
        }

        bool TileFilter(GOTile tile, Coordinates coordinates)
        {
            Vector2 tileCoordinates = coordinates.tileCoordinates(goMap.zoomLevel);
            bool isInTile = tile.goTile.tileCoordinates.Equals(tileCoordinates);
            if (debug) Debug.Log($"GO_PLACES2: TileFilter - Coordinates {coordinates} in tile: {isInTile}");
            return isInTile;
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
