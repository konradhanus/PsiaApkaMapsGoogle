using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class ResourceData
{
    public string gold;
    public string diamond;
    public string chicken;
    public string ball;
    public string water;
}

[System.Serializable]
public class ResourceResponse
{
    public List<ResourceData> data;
}

public class GetFoodData : MonoBehaviour
{
    private string url = "https://psiaapka.pl/resources.php?user_id=eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    private string updateUrl = "https://psiaapka.pl/resourcesUtilizate.php?user_id=eOexsqawm4YO9GhnmYT9Ka7RbRq1&resources_name={0}&resources_quantity={1}";

    public TextMeshProUGUI textChicken;
    public TextMeshProUGUI textWater;

    void Start()
    {
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error fetching data: " + webRequest.error);
            }
            else
            {
                // Parse the response
                ResourceResponse resourceResponse = JsonUtility.FromJson<ResourceResponse>(webRequest.downloadHandler.text);
                HandleData(resourceResponse);
                Debug.Log("FETCH YES");
            }
        }
    }

    void HandleData(ResourceResponse resourceResponse)
    {
        foreach (var data in resourceResponse.data)
        {
            Debug.Log("Gold: " + data.gold);
            Debug.Log("Diamond: " + data.diamond);
            Debug.Log("Chicken: " + data.chicken);
            Debug.Log("Ball: " + data.ball);
            Debug.Log("Water: " + data.water);

            // Assign values to TextMeshPro text fields
            if (textChicken != null)
            {
                textChicken.text = data.chicken;
            }

            if (textWater != null)
            {
                textWater.text = data.water;
            }


        }
    }

    public IEnumerator UpdateResource(string resourceName, int quantity)
    {
        string formattedUrl = string.Format(updateUrl, resourceName, quantity);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(formattedUrl))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Error updating resource: " + webRequest.error);
            }
            else
            {
                // Parse the response
                ResourceResponse resourceResponse = JsonUtility.FromJson<ResourceResponse>(webRequest.downloadHandler.text);
                HandleData(resourceResponse);
            }
        }
    }
}
