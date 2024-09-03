using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Dodaj to na początku pliku, aby używać TextMeshPro

public class PanelGetData : MonoBehaviour
{
    public GameObject WalkStar1;
    public GameObject WalkStar2;
    public GameObject WalkStar3;

    public GameObject PlayStar1;
    public GameObject PlayStar2;
    public GameObject PlayStar3;

    public GameObject TreatStar1;
    public GameObject TreatStar2;
    public GameObject TreatStar3;

    public GameObject TreasureStar1;
    public GameObject TreasureStar2;
    public GameObject TreasureStar3;

    public GameObject WaterStar1;
    public GameObject WaterStar2;
    public GameObject WaterStar3;

    public GameObject Resources; // Obiekt, który zawiera GlobalData

    public TextMeshProUGUI UUIDText; // Dodaj to pole dla TextMeshPro

    private string uuid; // UUID przypisywane z GlobalData

    private const string ApiUrl = "https://psiaapka.pl/getTasks.php?uuid=";

    void Start()
    {
        // Pobranie UserId z GlobalData i ustawienie UUID
        GlobalData globalData = Resources.GetComponent<GlobalData>();
        if (globalData != null)
        {
            uuid = globalData.userId; // Przypisanie UserId do zmiennej uuid
            // Ustawienie UUID w TextMeshPro
            if (UUIDText != null)
            {
                UUIDText.text = "UUID: " + uuid;
            }
            StartCoroutine(FetchAndUpdateStars()); // Wywołanie funkcji pobierającej dane
        }
        else
        {
            Debug.LogError("GlobalData component is missing on the Resources GameObject.");
        }
    }

    // Funkcja do pobierania danych z API i aktualizowania gwiazdek
    private IEnumerator FetchAndUpdateStars()
    {
        string url = ApiUrl + uuid;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                // Przetwarzanie danych JSON
                string jsonResponse = webRequest.downloadHandler.text;
                TaskResponse response = JsonUtility.FromJson<TaskResponse>(jsonResponse);

                if (response.tasks.Length > 0)
                {
                    var task = response.tasks[0]; // Zakładamy, że interesuje nas pierwszy wynik

                    // Przypisanie wartości do zmiennych
                    int walk = int.Parse(task.walk);
                    int play = int.Parse(task.play);
                    int treat = int.Parse(task.treat);
                    int treasure = int.Parse(task.treasure);
                    int water = int.Parse(task.water);

                    // Wywołanie funkcji ustawiającej widoczność gwiazdek
                    SetStars(walk, play, treat, treasure, water);
                }
                else
                {
                    SetStars(0, 0, 0, 0, 0);
                    Debug.LogWarning("Brak danych dla UUID: " + uuid);
                }
            }
            else
            {
                SetStars(0, 0, 0, 0, 0);
                Debug.LogError("Błąd pobierania danych: " + webRequest.error);
            }
        }
    }

    // Funkcja do ustawienia widoczności gwiazdek
    public void SetStars(int walk, int play, int treat, int treasure, int water)
    {
        // Resetowanie widoczności wszystkich gwiazdek
        SetStarVisibility(WalkStar1, WalkStar2, WalkStar3, walk);
        SetStarVisibility(PlayStar1, PlayStar2, PlayStar3, play);
        SetStarVisibility(TreatStar1, TreatStar2, TreatStar3, treat);
        SetStarVisibility(TreasureStar1, TreasureStar2, TreasureStar3, treasure);
        SetStarVisibility(WaterStar1, WaterStar2, WaterStar3, water);
    }

    // Pomocnicza funkcja do ustawiania widoczności gwiazdek
    private void SetStarVisibility(GameObject star1, GameObject star2, GameObject star3, int starCount)
    {
        star1.SetActive(starCount >= 1);
        star2.SetActive(starCount >= 2);
        star3.SetActive(starCount >= 3);
    }

    // Klasa pomocnicza do deserializacji odpowiedzi JSON
    [System.Serializable]
    public class TaskResponse
    {
        public Task[] tasks;
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
}
