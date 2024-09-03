using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GetBallNew : MonoBehaviour
{
    private string url = "https://psiaapka.pl/resources.php?user_id={0}";
    private string updateUrl = "https://psiaapka.pl/resourcesUtilizate.php?user_id={2}&resources_name={0}&resources_quantity={1}";

    public TextMeshProUGUI textBall;
    private FirebaseAuthManager authManager;
    public string userId = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    public GameObject Ball;
    public int ballValue = 0;

    void Start()
    {
        authManager = new FirebaseAuthManager();
        // Wykonaj call do API i zaktualizuj dane
        userId = ReferencesUserFirebase.userId;

        print("Player Id food:" + userId);


        Debug.Log("FETCH! JESTEM PONOWNIE!");

        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        string formattedUrl = string.Format(url, userId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(formattedUrl))
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
            if (textBall != null)
            {
                textBall.text = data.ball;
                if (int.TryParse(data.ball, out ballValue))
                {
                    Debug.Log("BALL " + ballValue);

                    if (ballValue <= 0)
                    {
                        Invoke("DisableBall", 3f);
                    }
                    else {
                        EnableBall();
                    }
                }
            }

        }
    }

    void DisableBall()
    {
        Ball.SetActive(false);
    }

    void EnableBall()
    {
        Ball.SetActive(true);
    }

    public IEnumerator UpdateResource(string resourceName, int quantity)
    {
        string formattedUrl = string.Format(updateUrl, resourceName, quantity, userId);

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
