using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Dodaj to na początku pliku, aby używać TextMeshPro
using UnityEngine.UI; // Dodaj to na początku pliku, aby używać Slider
using System.Collections.Generic;



public class PanelGetData : MonoBehaviour
{
    [System.Serializable]
    public class UIElementData
    {
        public string name;
        public GameObject playButtonEnableGetPrize;
        public GameObject playButtonDisableGetPrize;
        public GameObject listObject; // Obiekt GameObject z komponentem Image
        public Sprite enableImageListBackground;
        public Sprite disableImageListBackground;
        public Slider taskCompletionSlider;
        public TextMeshProUGUI textInfo;
        public GameObject thumbsUp;
    }


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


    public List<UIElementData> uiElements;

    // Publiczne przyciski
    public GameObject playButtonEnableGetPrize;
    public GameObject playButtonDisableGetPrize;

    public GameObject List1; // Obiekt GameObject z komponentem Image

    public Sprite enableImageListBackground;
    public Sprite disableImageListBackground;


    public Slider taskCompletionSlider; // Dodaj to pole dla suwaka
    public TextMeshProUGUI Text_Info; // Dodaj to pole dla TextMeshPro

    public GameObject Resources; // Obiekt, który zawiera GlobalData
    public TextMeshProUGUI UUIDText; // Dodaj to pole dla TextMeshPro

    public bool enableDebugLogs = true; // Dodaj to pole, aby kontrolować wyświetlanie logów

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
            if (enableDebugLogs)
                Debug.LogError("PanelGetData: GlobalData component is missing on the Resources GameObject.");
        }
    }

    // Dodaj tę funkcję do swojej klasy PanelGetData
    public void FetchDataFromApi()
    {
        Debug.Log("PanelGetData: FECHDATAFROMAPI");
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
            if (enableDebugLogs)
                Debug.LogError("PanelGetData: GlobalData component is missing on the Resources GameObject.");
        }

    }

    // Funkcja do pobierania danych z API i aktualizowania gwiazdek
    private IEnumerator FetchAndUpdateStars()
    {
        string url = ApiUrl + uuid;

        Debug.Log("PanelGetData: " + url);

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
                    int treat =  int.Parse(task.treat);
                    int treasure = int.Parse(task.treasure);
                    int water = int.Parse(task.water);

                    // Wywołanie funkcji ustawiającej widoczność gwiazdek
                    SetStars(walk, play, treat, treasure, water);
                }
                else
                {
                    SetStars(0, 0, 0, 0, 0);
                    if (enableDebugLogs)
                        Debug.LogWarning("PanelGetData: Brak danych dla UUID: " + uuid);
                }
            }
            else
            {
                SetStars(0, 0, 0, 0, 0);
                if (enableDebugLogs)
                    Debug.LogError("PanelGetData: Błąd pobierania danych: " + webRequest.error);
            }
        }
    }

    // Funkcja do ustawienia widoczności gwiazdek
    public void SetStars(int walk, int play, int treat, int treasure, int water)
    {
        // Resetowanie widoczności wszystkich gwiazdek
        SetStarVisibility(WalkStar1, WalkStar2, WalkStar3, walk);
        SetStarVisibility(PlayStar1, PlayStar2, PlayStar3, play);

        if (Text_Info != null)
        {
            Text_Info.text = play + " <#b3bedb>/ 3";
        }

        // Zmiana Source Image na "ListFrame02_d" w obiekcie List
        if (List1 != null)
        {
            Image listImage = List1.GetComponent<Image>();
            if (listImage != null)
            {
                if (play >= 3)
                {
                    listImage.sprite = enableImageListBackground;
                    playButtonEnableGetPrize.SetActive(true);
                    playButtonDisableGetPrize.SetActive(false);
                }
                else {
                    listImage.sprite = disableImageListBackground;
                    playButtonEnableGetPrize.SetActive(false);
                    playButtonDisableGetPrize.SetActive(true);
                }
                
            }
            else
            {
                Debug.LogError("PanelGetData: Image component is missing on the List GameObject.");
            }
        }
        else
        {
            Debug.LogError("PanelGetData: List GameObject is not assigned.");
        }

        foreach (UIElementData element in uiElements)
        {

            if (element.name == "play")
            {
                setAll(element, play);
            }

            if (element.name == "treat")
            {
                setAll(element, treat);
            }

            if (element.name == "treasure")
            {
                setAll(element, treasure);
            }

            if (element.name == "water")
            {
                setAll(element, water);
            }

            if (element.name == "walk")
            {

                setAll(element, walk);
               
                
            }

            // Przykład: Zmiana tła dla pierwszego elementu
            

            

            // Przykład: Zmiana tekstu
            //element.textInfo.text = "Zadanie w trakcie realizacji";
        }

        if (enableDebugLogs)
            Debug.Log("PanelGetData: dla UUID: " + uuid + " jest "+ play);


        SetStarVisibility(TreatStar1, TreatStar2, TreatStar3, treat);
        SetStarVisibility(TreasureStar1, TreasureStar2, TreasureStar3, treasure);
        SetStarVisibility(WaterStar1, WaterStar2, WaterStar3, water);
    }

    private void setAll(UIElementData element, float task)
    {

        element.taskCompletionSlider.value = task / 3f;
        
        if (task >= 3)
        {
            element.listObject.GetComponent<Image>().sprite = element.enableImageListBackground;
            element.playButtonEnableGetPrize.SetActive(false);
            element.playButtonDisableGetPrize.SetActive(true);
            element.thumbsUp.SetActive(false);
        }
        else
        {
            element.listObject.GetComponent<Image>().sprite = element.disableImageListBackground;
            
            element.playButtonEnableGetPrize.SetActive(true);
            element.playButtonDisableGetPrize.SetActive(false);
            element.thumbsUp.SetActive(false);

        }

        if (element.name == "walk")
        {
            element.textInfo.text = task + " km <#b3bedb>/ 3 km";
        }
        else {
            element.textInfo.text = task + " <#b3bedb>/ 3";
        }

    
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
