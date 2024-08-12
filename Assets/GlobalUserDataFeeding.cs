using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalUserData;
using UnityEngine.Networking;
using Mapbox.Map;

public class GlobalUserDataFeeding : MonoBehaviour
{

    public GameObject[] Dogs; // Tablica dla wszystkich ps?w

    public string userId = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";

    private FirebaseAuthManager authManager;

    [System.Serializable]
    public class DataResponse
    {
        public UserData data;
    }

    [System.Serializable]
    public class UserData
    {
        public string nick;
        public int id_avatar;
        public int id_avatar_dog;
        public string date_created;
    }

    UserData userData;

    // Start is called before the first frame update
    void Start()
    {
        
        authManager = new FirebaseAuthManager();
        userId = ReferencesUserFirebase.userId;

        print("Player Id:" + userId);

    
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
            Debug.LogError("B??d podczas pobierania danych z API: " + request.error);
            yield break;
        }

        string jsonResponse = request.downloadHandler.text;
        DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(jsonResponse);

        if (dataResponse != null && dataResponse.data != null)
        {
            Debug.Log("FETCH!!!" + dataResponse);
            userData = dataResponse.data;
            UpdateUserData(dataResponse.data);
        }
        else
        {
            Debug.LogError("B??d podczas parsowania odpowiedzi z API.");
        }
    }

    public void UpdateUserData(UserData userData)
    {
        Debug.Log("FETCH! Nick: " + userData.nick + ", SkinId: " + userData.id_avatar + ", DogSkinId: " + userData.id_avatar_dog + ", Registration Date: " + userData.date_created);

        // Update UI elements with received data
        // nickNameText.text = userData.nick;

        // Update avatar and character visibility based on skinId
        Debug.Log("FETCH!" + userData.id_avatar);

        // Update the visibility of the dogs based on the id_avatar_dog
        ShowDog(userData.id_avatar_dog);
    }


    private void ShowDog(int dogId)
    {
        // Ukryj wszystkie psy
        foreach (GameObject dog in Dogs)
        {
            dog.SetActive(false);
        }
        int index;
        if (dogId >= 0 && dogId <= Dogs.Length)
        {
            index = dogId; // Przekszta?cenie dogId (1-28) na indeks (0-27)
        }
        else
        {
            index = 24; // Domy?lnie ustaw psa nr 14 (indeks 13)
        }

        Dogs[index].SetActive(true);

    }


}
