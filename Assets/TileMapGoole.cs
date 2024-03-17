using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public class GoogleMapsTilemap : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase tile;

    public string centerLatitude = "35.6209411621094";
    public string centerLongitude = "139.718521118164";
    public int zoom = 16;
    public int size = 2000;
    public int scale = 2;
    public string mapType = "roadmap";

    public string apiKey = "AIzaSyC2AUF6w945MZEvC3OxGgndgWS3EFCk4Lo";

    void Start()
    {
        StartCoroutine(LoadMap());
    }

    IEnumerator LoadMap()
    {
        string url = $"https://maps.googleapis.com/maps/api/staticmap?center={centerLatitude},{centerLongitude}&zoom={zoom}&size={size}x{size}&scale={scale}&maptype={mapType}&sensor=false&key={apiKey}";

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                // Utwórz nowy kafelek z pobraną teksturą
                Tile tileInstance = ScriptableObject.CreateInstance<Tile>();
                tileInstance.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

                // Clear existing tiles
                tilemap.ClearAllTiles();

                // Ustaw pobrany kafelek jako jedyny kafelek na mapie
                tilemap.SetTile(new Vector3Int(0, 0, 0), tileInstance);
            }
        }
    }
}
