using UnityEngine;
using System.Collections;
using GoShared;

namespace GoMap
{
    public class GoPlaces2 : MonoBehaviour
    {
        public GOMap goMap;
        public GameObject prefab;
        private GameObject placedObject; // Store the reference to the instantiated object

        void Awake()
        {
            // Register to the GOMap event OnTileLoad
            goMap.OnTileLoad.AddListener((GOTile) => {
                OnLoadTile(GOTile);
            });
        }

        void OnLoadTile(GOTile tile)
        {
            // Only place the object if it hasn't been placed yet
            if (placedObject == null)
            {
                NearbySearch(tile);
            }
        }

        void NearbySearch(GOTile tile)
        {
            Debug.Log("TILE");

            // Set coordinates (latitude and longitude)
            double lat = 51.086570739746094;
            double lng = 17.053138732910156;

            // Create a new coordinate object with the desired lat/lon
            Coordinates coordinates = new Coordinates(lat, lng, 0);

            if (TileFilter(tile, coordinates))
            {
                // Instantiate your game object if it hasn't been instantiated yet
                placedObject = Instantiate(prefab);
                placedObject.SetActive(true);

                // Convert coordinates to position
                Vector3 position = coordinates.convertCoordinateToVector(placedObject.transform.position.y);
                Debug.Log("TILE Converted Position: " + position.ToString());

                if (goMap.useElevation)
                    position = GOMap.AltitudeToPoint(position);

                // Set the position of the object
                placedObject.transform.localPosition = position;

                // Set the parent of the object
                placedObject.transform.SetParent(tile.transform);

                // Optionally set the name of the object
                placedObject.name = "PlacedObject";
            }
        }

        bool TileFilter(GOTile tile, Coordinates coordinates)
        {
            Vector2 tileCoordinates = coordinates.tileCoordinates(goMap.zoomLevel);

            // Ensure the coordinates are within the tile
            return tile.goTile.tileCoordinates.Equals(tileCoordinates);
        }
    }
}
