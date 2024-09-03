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

    public void OnUpdateButtonBallClick()
    {
        StartCoroutine(UpdateTaskDataBall(userId));
    }

    public void OnUpdateButtonFoodAndWaterClick()
    {
        StartCoroutine(UpdateTaskDataFoodAndWater(userId));
    }


    [System.Serializable]
    public class Task
    {
        public string id;
        public string UUID;
        public string walk;
        public string play;
        public string treat;
        public string treasure;
        public string water;
        public string date;
    }

    [System.Serializable]
    public class TaskResponse
    {
        public List<Task> tasks;
    }

    IEnumerator UpdateTaskDataBall(string userId)
    {
        

        // 1. Pobierz aktualny stan z API
        string fetchUrl = "https://psiaapka.pl/getTasks.php?uuid=" + userId;
        Debug.Log("UPDATE: " + fetchUrl);
        UnityWebRequest fetchRequest = UnityWebRequest.Get(fetchUrl);
        yield return fetchRequest.SendWebRequest();

        if (fetchRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("UPDATE: Błąd podczas pobierania danych z API: " + fetchRequest.error);
            yield break;
        }

        string fetchJsonResponse = fetchRequest.downloadHandler.text;
        TaskResponse taskResponse = JsonUtility.FromJson<TaskResponse>(fetchJsonResponse);

        // Sprawdź, czy odpowiedź zawiera zadania
        string walkValue, playValue, treatValue, treasureValue, waterValue;

        if (taskResponse == null || taskResponse.tasks.Count == 0)
        {
            Debug.Log("UPDATE: Brak zadań w odpowiedzi z API. Ustawiam domyślne wartości.");
            // Ustaw domyślne wartości
            walkValue = "0";
            playValue = "1"; // Ustaw playValue na 1, gdy brak danych
            treatValue = "0";
            treasureValue = "0";
            waterValue = "0";
        }
        else
        {
            Task task = taskResponse.tasks[0]; // Zakładam, że interesuje Cię pierwsze zadanie
            walkValue = task.walk;
            playValue = (int.Parse(task.play) + 1).ToString(); // Zwiększ playValue o 1
            treatValue = task.treat;
            treasureValue = task.treasure;
            waterValue = task.water;
        }

        // Użyj UUID z userId2 (zakładając, że jest to UUID)
        string updateUrl = $"https://psiaapka.pl/updateTasks.php?walk={walkValue}&play={playValue}&treat={treatValue}&treasure={treasureValue}&water={waterValue}&uuid={userId}";
        UnityWebRequest updateRequest = UnityWebRequest.Get(updateUrl);
        yield return updateRequest.SendWebRequest();

        if (updateRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("UPDATE: Błąd podczas aktualizacji danych z API: " + updateRequest.error);
        }
        else
        {
            Debug.Log("UPDATE: Dane zostały pomyślnie zaktualizowane.");
        }
    }

    IEnumerator UpdateTaskDataFoodAndWater(string userId)
    {


        // 1. Pobierz aktualny stan z API
        string fetchUrl = "https://psiaapka.pl/getTasks.php?uuid=" + userId;
        Debug.Log("UPDATE: " + fetchUrl);
        UnityWebRequest fetchRequest = UnityWebRequest.Get(fetchUrl);
        yield return fetchRequest.SendWebRequest();

        if (fetchRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("UPDATE: Błąd podczas pobierania danych z API: " + fetchRequest.error);
            yield break;
        }

        string fetchJsonResponse = fetchRequest.downloadHandler.text;
        TaskResponse taskResponse = JsonUtility.FromJson<TaskResponse>(fetchJsonResponse);

        // Sprawdź, czy odpowiedź zawiera zadania
        string walkValue, playValue, treatValue, treasureValue, waterValue;

        if (taskResponse == null || taskResponse.tasks.Count == 0)
        {
            Debug.Log("UPDATE: Brak zadań w odpowiedzi z API. Ustawiam domyślne wartości.");
            // Ustaw domyślne wartości
            walkValue = "0";
            playValue = "0"; // Ustaw playValue na 1, gdy brak danych
            treatValue = "1";
            treasureValue = "0";
            waterValue = "1";
        }
        else
        {
            Task task = taskResponse.tasks[0]; // Zakładam, że interesuje Cię pierwsze zadanie
            walkValue = task.walk;
            playValue = task.play; // Zwiększ playValue o 1
            treatValue = (int.Parse(task.treat) + 1).ToString(); ;
            treasureValue = task.treasure;
            waterValue = (int.Parse(task.water) + 1).ToString(); ;
        }

        // Użyj UUID z userId2 (zakładając, że jest to UUID)
        string updateUrl = $"https://psiaapka.pl/updateTasks.php?walk={walkValue}&play={playValue}&treat={treatValue}&treasure={treasureValue}&water={waterValue}&uuid={userId}";
        UnityWebRequest updateRequest = UnityWebRequest.Get(updateUrl);
        yield return updateRequest.SendWebRequest();

        if (updateRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("UPDATE: Błąd podczas aktualizacji danych z API: " + updateRequest.error);
        }
        else
        {
            Debug.Log("UPDATE: Dane zostały pomyślnie zaktualizowane.");
        }
    }

}
