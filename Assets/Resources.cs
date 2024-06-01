using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GlobalData : MonoBehaviour
{
    public bool debugMode = true;
    public static GlobalData Instance;
    // public TextMeshProUGUI nickName;
    public Text logTxt;
    // Dane z API
    public int gold = 0;
    public int diamond = 0;
    public int chicken = 0;
    public int ball = 0;
    public int water = 0;
    public string userId = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    private FirebaseAuthManager authManager;


    // TextMeshPro references
    public TextMeshProUGUI chickenText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI ballText;
    public TextMeshProUGUI coinText;

    // Struktura do przechowywania danych z JSON-a
    [System.Serializable]
    public class DataResponse
    {
        public Data[] data;
    }

    [System.Serializable]
    public class Data
    {
        public string gold;
        public string diamond;
        public string chicken;
        public string ball;
        public string water;
    }
    
    void Start()
    {
        if(debugMode)
        {

        }else{
            authManager = new FirebaseAuthManager();
            // Wykonaj call do API i zaktualizuj dane
            userId = ReferencesUserFirebase.userId;
    
            print("Player Id:" + userId);
            logTxt.text = userId;
            // nickName.text = ReferencesUserFirebase.userName;
        }
        Debug.Log("FETCH! JESTEM PONOWNIE!");
        StartCoroutine(UpdateDataFromAPI());
    }


    public void FetchData() {
        Debug.Log("FETCH!, fetch data");
        Debug.Log("userId"+ userId);
        StartCoroutine(UpdateDataFromAPI());
    }
    IEnumerator UpdateDataFromAPI()
    {
        // URL do API
        string url = "https://psiaapka.pl/resources.php?user_id="+userId;

        Debug.Log("FETCH! "+url);
        // Wykonaj call do API
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        // Sprawdź, czy wystąpił błąd podczas pobierania danych
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Błąd podczas pobierania danych z API: " + request.error);
            yield break;
        }

        // Parsuj otrzymany JSON
        string jsonResponse = request.downloadHandler.text;
        DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(jsonResponse);

        // Aktualizuj dane na podstawie otrzymanego JSON-a
        if (dataResponse != null && dataResponse.data.Length > 0)
        {
            Debug.Log("FETCH!");
            UpdateData(int.Parse(dataResponse.data[0].gold), int.Parse(dataResponse.data[0].diamond), int.Parse(dataResponse.data[0].chicken), int.Parse(dataResponse.data[0].ball), int.Parse(dataResponse.data[0].water));
        }
        else
        {
            Debug.LogError("Błąd podczas parsowania odpowiedzi z API.");
        }
    }

    // void Awake()
    // {
    //     // Sprawdź, czy istnieje już instancja tego obiektu
    //     if (Instance == null)
    //     {
    //         // Jeśli nie, ustaw ten obiekt jako instancję
    //         Instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         // Jeśli instancja już istnieje, zniszcz ten obiekt, aby uniknąć duplikatów
    //         Destroy(gameObject);
    //     }
    // }

    // Metoda do aktualizacji danych z odpowiedzi API
    public void UpdateData(int newGold, int newDiamond, int newChicken, int newBall, int newWater)
    {
        Debug.Log("FETCH! gold:"+newGold+", diamond:"+newDiamond+", chicken: "+newChicken+", ball:"+newBall+", water:"+newWater);
        gold = newGold;
        diamond = newDiamond;
        chicken = newChicken;
        ball = newBall;
        water = newWater;

        chickenText.text = chicken.ToString();
        diamondText.text = diamond.ToString();
        waterText.text =  water.ToString();
        ballText.text =  ball.ToString();
        coinText.text =  gold.ToString();
    }

     // Metoda do odczytywania danych
    public void ReadData(out int goldValue, out int diamondValue, out int chickenValue, out int ballValue, out int waterValue)
    {
        goldValue = gold;
        diamondValue = diamond;
        chickenValue = chicken;
        ballValue = ball;
        waterValue = water;
    }
}
