using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    [SerializeField]
    public float latitude;

    [SerializeField]
    public float longitude;

    [SerializeField, Range(1, 30)]
    private int zoom = 18;

    [SerializeField, Range(1, 30)]
    private int scale = 2;


    public void LoadMap()
    {
        string imageUrl = GetMapImageUrl();
        StartCoroutine(LoadImage(imageUrl));
    }


    string GetMapImageUrl()
    {
        string center = $"{latitude},{longitude}";
        string scaleParam = $"&scale={scale}";
        string zoomParam = $"&zoom={zoom}";
        // Utwórz URL dla Google Static Maps API na podstawie współrzędnych Latitude i Longitude.
        // Dodaj dodatkowe parametry, jeśli to konieczne.

        string imageUrl = "https://maps.googleapis.com/maps/api/staticmap?center=" + center +
                          ""+ zoomParam + "&size=2000x2000"+ scaleParam + "&maptype=roadmap&sensor=false" +
                          "&style=element:labels.text.fill|visibility:off&style=element:labels.text.stroke|visibility:off" +
                          "&style=element:labels.icon|visibility:off&style=feature:landscape.man_made|element:geometry.fill|color:0xa1f199" +
                          "&style=feature:landscape.natural.landcover|element:geometry.fill|color:0x37bda2" +
                          "&style=feature:landscape.natural.terrain|element:geometry.fill|color:0x37bda2" +
                          "&style=feature:poi.attraction|element:geometry.fill|visibility:on" +
                          "&style=feature:poi.business|element:geometry.fill|color:0xe4dfd9" +
                          "&style=feature:poi.business|element:labels.icon|visibility:off" +
                          "&style=feature:poi.park|element:geometry.fill|color:0x37bda2" +
                          "&style=feature:road|element:geometry.fill|color:0x84b09e" +
                          "&style=feature:road|element:geometry.stroke|color:0xfafeb8|weight:1.25" +
                          "&style=feature:road.highway|element:labels.icon|visibility:off" +
                          "&style=feature:water|element:geometry.fill|color:0x5ddad6" +
                          "&key=AIzaSyC2AUF6w945MZEvC3OxGgndgWS3EFCk4Lo";

        return imageUrl;
    }



    void Start()
    {
        LoadMap();
    }

    IEnumerator LoadImage(string imageUrl)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // Pobrano obrazek, możesz go teraz przypisać do materiału lub użyć w inny sposób.
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                ApplyTextureToMaterial(texture);
            }
        }
    }

    void ApplyTextureToMaterial(Texture2D texture)
    {
        // Tutaj możesz przypisać teksturę do materiału na planszy.
        // Na przykład, jeśli plansza ma komponent Renderer:
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.mainTexture = texture;
        }
    }
}