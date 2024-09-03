using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class ResourceDataBall
{
    public string gold;
    public string diamond;
    public string chicken;
    public string ball;
    public string water;
}

[System.Serializable]
public class ResourceResponseBall
{
    public List<ResourceDataBall> data;
}

public class GetFoodData : MonoBehaviour
{
    private string url = "https://psiaapka.pl/resources.php?user_id={0}";
    private string updateUrl = "https://psiaapka.pl/resourcesUtilizate.php?user_id={2}&resources_name={0}&resources_quantity={1}";

    public TextMeshProUGUI textChicken;
    public TextMeshProUGUI textWater;
    private FirebaseAuthManager authManager;
    public string userId = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    public int chickenValue = 0;
    public int waterValue = 0;
    public GameObject Chicken;
    public GameObject Water;
    public GameObject ChickenWaterContainer; 

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
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching data: " + webRequest.error);
            }
            else
            {
                ResourceResponseBall resourceResponse = JsonUtility.FromJson<ResourceResponseBall>(webRequest.downloadHandler.text);
                HandleData(resourceResponse);
                Debug.Log("FETCH YES");
            }
        }
    }

    void HandleData(ResourceResponseBall resourceResponse)
    {
        ThrowBall throwBallScript = ChickenWaterContainer.GetComponent<ThrowBall>();

        

        foreach (var data in resourceResponse.data)
        {
            Debug.Log("Gold: " + data.gold);
            Debug.Log("Diamond: " + data.diamond);
            Debug.Log("Chicken: " + data.chicken);
            Debug.Log("Ball: " + data.ball);
            Debug.Log("Water: " + data.water);

            if (textChicken != null)
            {
                textChicken.text = data.chicken;

                if (int.TryParse(data.chicken, out chickenValue))
                {
                    if (chickenValue <= 0)
                    {
                        Chicken.SetActive(false);


                        if (throwBallScript != null)
                        {
                        
                            bool isChicken = throwBallScript.isChicken;

                            if (isChicken)
                            {
                                ChickenWaterContainer.SetActive(false);
                            }
                        }
                        else
                        {
                            Debug.LogError("ThrowBall script not found on ChickenWaterContainer.");
                        }

                        //active 
                       // ChickenWaterContainer.SetActive(false);
                    }
                }
            }

            if (textWater != null)
            {
                textWater.text = data.water;

                if (int.TryParse(data.water, out waterValue))
                {
                    if (waterValue <= 0)
                    {
                        Water.SetActive(false);

                        if (throwBallScript != null)
                        {

                            bool isWater = throwBallScript.isWater;

                            if (isWater)
                            {
                                ChickenWaterContainer.SetActive(false);
                            }
                        }
                        else
                        {
                            Debug.LogError("ThrowBall script not found on ChickenWaterContainer.");
                        }
                    }
                }
            }
        }
    }

    public IEnumerator UpdateResource(string resourceName, int quantity)
    {
        yield return new WaitForSeconds(3f);
        string formattedUrl = string.Format(updateUrl, resourceName, quantity, userId);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(formattedUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error updating resource: " + webRequest.error);
            }
            else
            {
                ResourceResponseBall resourceResponse = JsonUtility.FromJson<ResourceResponseBall>(webRequest.downloadHandler.text);
                HandleData(resourceResponse);
            }
        }
    }
}
