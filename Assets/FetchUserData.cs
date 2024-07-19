using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class GlobalUserData : MonoBehaviour
{
    public bool debugMode = true;
    public static GlobalUserData Instance;

    public string userId = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    private FirebaseAuthManager authManager;

    UserData userData;

    public TextMeshProUGUI nickNameText;

    [System.Serializable]
    public class DataResponse
    {
        public UserData data;
    }

    [System.Serializable]
    public class UserData
    {
        public string nick;
        public int skinId;
        public int dogSkinId;
        public string date_created;
    }

    void Start()
    {
        if (debugMode)
        {
            // Debug mode logic
        }
        else
        {
            authManager = new FirebaseAuthManager();
            userId = ReferencesUserFirebase.userId;

            print("Player Id:" + userId);

        }
        Debug.Log("FETCH! JESTEM PONOWNIE!");
        StartCoroutine(UpdateDataFromAPI());
    }

    public void FetchData()
    {
        Debug.Log("FETCH!, fetch data");
        Debug.Log("userId" + userId);
        StartCoroutine(UpdateDataFromAPI());
    }

    IEnumerator UpdateDataFromAPI()
    {
        string url = "https://psiaapka.pl/psiaapka/userData.php?user_id=" + userId;

        Debug.Log("FETCH! " + url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("B³¹d podczas pobierania danych z API: " + request.error);
            yield break;
        }

        string jsonResponse = request.downloadHandler.text;
        DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(jsonResponse);

        if (dataResponse != null && dataResponse.data != null)
        {
            Debug.Log("FETCH!!!"+ dataResponse);
            userData = dataResponse.data;
            UpdateUserData(dataResponse.data);
        }
        else
        {
            Debug.LogError("B³¹d podczas parsowania odpowiedzi z API.");
        }
    }

    public void UpdateUserData(UserData userData)
    {
        Debug.Log("FETCH! Nick: " + userData.nick + ", SkinId: " + userData.skinId + ", DogSkinId: " + userData.dogSkinId + ", Registration Date: " + userData.date_created);

        // Update UI elements with received data
        nickNameText.text = userData.nick;
        // Assign other data to UI or other variables as needed
    }

    public void ReadData(out string nick, out int skinId, out int dogSkinId, out string date_created)
    {
        // Return data from API response
        nick = nickNameText.text;
        // Directly assign the integer values from userData
        skinId = userData.skinId;
        dogSkinId = userData.dogSkinId;
        date_created = userData.date_created;
    }
}
