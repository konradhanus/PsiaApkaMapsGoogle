using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro; // Dodaj to na początku pliku, aby używać TextMeshPro
using UnityEngine.UI; // Dodaj to na początku pliku, aby używać Slider
using System.Collections.Generic;
using System.Net.Http;
using System.Globalization;

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

    public TextMeshProUGUI todayWalk;
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

    public TextMeshProUGUI UUID_Text;


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

    public string uuid; // UUID przypisywane z GlobalData
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
                UUIDText.text = " " + uuid;
            }

            if (UUID_Text != null)
            {
                UUID_Text.text = " " + uuid;
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
                UUIDText.text = " " + uuid;
            }

            if (UUID_Text != null)
            {
                UUID_Text.text = " " + uuid;
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

                    int reward_walk = int.Parse(task.reward_walk);
                    int reward_play = int.Parse(task.reward_play);
                    int reward_treat = int.Parse(task.reward_treat);
                    int reward_treasure = int.Parse(task.reward_treasure);
                    int reward_water = int.Parse(task.reward_water);


                    Debug.Log("TO CO PRZYSZŁO Z API"+ reward_walk + " " + reward_play +" "+reward_treasure + " " + reward_treat + " " + reward_water);

                    // Wywołanie funkcji ustawiającej widoczność gwiazdek
                    SetStars(walk, play, treat, treasure, water, reward_walk, reward_play, reward_treat, reward_treasure, reward_water);
                }
                else
                {
                    SetStars(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    if (enableDebugLogs)
                        Debug.LogWarning("PanelGetData: Brak danych dla UUID: " + uuid);
                }
            }
            else
            {
                SetStars(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                if (enableDebugLogs)
                    Debug.LogError("PanelGetData: Błąd pobierania danych: " + webRequest.error);
            }
        }
    }

    // Funkcja do ustawienia widoczności gwiazdek
    public void SetStars(int walk, int play, int treat, int treasure, int water, int reward_walk, int reward_play, int reward_treat, int reward_treasure, int reward_water)
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
                if (reward_play >= 1)
                {
                    element.thumbsUp.SetActive(true);
                    element.playButtonEnableGetPrize.SetActive(false);
                    element.playButtonDisableGetPrize.SetActive(false);
                }
            }

            if (element.name == "treat")
            {
                setAll(element, treat);
                if (reward_treat >= 1)
                {
                    element.thumbsUp.SetActive(true);
                    element.playButtonEnableGetPrize.SetActive(false);
                    element.playButtonDisableGetPrize.SetActive(false);
                }
            }

            if (element.name == "treasure")
            {
                setAll(element, treasure);
                if (reward_treasure >= 1)
                {
                    element.thumbsUp.SetActive(true);
                    element.playButtonEnableGetPrize.SetActive(false);
                    element.playButtonDisableGetPrize.SetActive(false);
                }
            }

            if (element.name == "water")
            {
                setAll(element, water);
                if (reward_water >= 1)
                {
                    element.thumbsUp.SetActive(true);
                    element.playButtonEnableGetPrize.SetActive(false);
                    element.playButtonDisableGetPrize.SetActive(false);
                }
            }

            if (element.name == "walk")
            {


                string distanceString = todayWalk.text;

                distanceString = distanceString.Replace(" km", "").Replace(",", ".");

                // Konwersja na float
                float distance = float.Parse(distanceString, CultureInfo.InvariantCulture);
                setAll(element, distance);
                if (reward_walk >= 1)
                {
                    element.thumbsUp.SetActive(true);
                    element.playButtonEnableGetPrize.SetActive(false);
                    element.playButtonDisableGetPrize.SetActive(false);
                }


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


        Debug.Log("SLIDER VALUE:"+ element.name +": " + task);
        Debug.Log("SLIDER VALUE2:" + element.name + ": "+ task / 3f);
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
            element.textInfo.text = todayWalk.text + " <#b3bedb>/ 3 km";
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

        public string reward_walk;
        public string reward_play;
        public string reward_treat;
        public string reward_treasure;
        public string reward_water;

        public string date;
    }


    private static readonly HttpClient httpClient = new HttpClient();

    private async void CallApi(string url)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Debug.Log("API response: " + responseBody);
            }
            else
            {
                Debug.LogError("Błąd w wywołaniu API: " + response.StatusCode);
            }
        }
        catch (HttpRequestException e)
        {
            Debug.LogError("Wystąpił wyjątek w czasie wywołania API: " + e.Message);
        }
    }

    // Metoda do ustawienia widoczności tylko ikony i wartości monety
    public async void ShowCoin(int value)
    {
        //getUUID();
        string url = $"https://psiaapka.pl/updateReward.php?reward_play=1&uuid={uuid}";
        // Wywołanie API
        CallApi(url);
        Debug.Log("AAAAAAA" + url);
        //ResetIcons();
        //coinIcon.SetActive(true);
        //title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
        //textValue.text = value.ToString();
    }

    // Metoda do ustawienia widoczności tylko ikony i wartości diamentu
    public async void ShowDiamondAsync(int value)
    {
        //getUUID();
        string url = $"https://psiaapka.pl/updateReward.php?reward_treat=1&uuid={uuid}";
        // Wywołanie API
        CallApi(url);
        Debug.Log("AAAAAAA" + url);
        //ResetIcons();
        //diamondIcon.SetActive(true);
        //title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
        //textValue.text = value.ToString();
    }

    // Metoda do ustawienia widoczności tylko ikony i wartości skrzyni
    public async void ShowChestAsync(int value)
    {
        //getUUID();
        // URL API, do którego zostanie wysłane zapytanie

        string url = $"https://psiaapka.pl/updateReward.php?reward_water=1&uuid={uuid}";

        Debug.Log("AAAAAAA" + url);
        // Wywołanie API
        CallApi(url);

        //ResetIcons();
        //chestIcon.SetActive(true);
        //title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
        //textValue.text = value.ToString();


    }

    // Metoda do ustawienia widoczności tylko ikony i wartości jedzenia
    public async void ShowFood(int value)
    {
        //getUUID();
        string url = $"https://psiaapka.pl/updateReward.php?reward_walk=1&uuid={uuid}";

        Debug.Log("AAAAAAA" + url);
        // Wywołanie API
        CallApi(url);

        //ResetIcons();
        //foodIcon.SetActive(true);
        //title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
        //textValue.text = value.ToString();
    }

    //// Metoda do ustawienia widoczności tylko ikony i wartości piłki
    //public void ShowBall(int value)
    //{
    //    ResetIcons();
    //    ballIcon.SetActive(true);
    //    title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
    //    textValue.text = value.ToString();
    //}

    // Metoda do ustawienia widoczności tylko ikony i wartości torby
    public async void ShowBag(int value)
    {

        Debug.Log("AAAAAAA");
        // getUUID();
        string url = $"https://psiaapka.pl/updateReward.php?reward_treasure=1&uuid={uuid}";
        Debug.Log("AAAAAAA" + url);
        // Wywołanie API
        CallApi(url);

        //ResetIcons();
        //bagIcon.SetActive(true); // Jeśli backIcon oznacza torbę, można to zmienić
        //title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
        //textValue.text = value.ToString();
    }

   

    //// Resetuje widoczność wszystkich ikon
    //private void ResetIcons()
    //{
    //    coinIcon.SetActive(false);
    //    diamondIcon.SetActive(false);
    //    bagIcon.SetActive(false);
    //    chestIcon.SetActive(false);
    //    foodIcon.SetActive(false);
    //    //ballIcon.SetActive(false);
    //}
}
