using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;


public class ClickDogSpot : MonoBehaviour
{
    private static bool isClicked = false; // Statyczna zmienna, aby przechowywać stan kliknięcia
    private static GameObject clickedObject = null; // Statyczna zmienna do przechowywania obiektu, który zablokował kliknięcia

    private string userId;
    private long lastDate;
    private string prevUserId;
    public GameObject dogspot;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public GameObject menuToDisable; // Menu do wyłączenia
    public GameObject menuToEnable; // Menu do wyłączenia
    public GameObject gemPrefab1; // Prefabrykat obiektu gem
    public GameObject gemPrefab2; // Prefabrykat obiektu gem
    public GameObject gemPrefab3; // Prefabrykat obiektu gem
    public GameObject gemPrefab4; // Prefabrykat obiektu gem
    public GameObject gemPrefab5; // Prefabrykat obiektu gem
    public GameObject[] gemPrefabs;

    // SAVE DATA
    private const string VisitedDogSpotId = "VisitedDogSpotId";

    private void SaveVisitedDogSpots()
    {
        long lastVisitTimestamp = GetCurrentTimestamp();
        string data = lastVisitTimestamp.ToString();
        string keyDogSpot = VisitedDogSpotId+id;
        PlayerPrefs.SetString(keyDogSpot, data);
        PlayerPrefs.Save();
        Debug.Log("SAVE: "+data);
    }

    public void AddVisitedDogSpot(int dogSpotId)
    {
        SaveVisitedDogSpots();
    }

    private long GetCurrentTimestamp()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public Material visitedDogSpotMaterial; // Nowy materiał, który chcesz przypisać dzieciom
   

    public float rotationSpeed = 50f; // Prędkość obrotu
    public float fastRotationSpeedMultiplier = 2f; // Mnożnik szybkości obrotu po kliknięciu
    public float fastRotationDuration = 1f; // Czas trwania szybkiego obrotu po kliknięciu
    public float accelerationTime = 0.5f; // Czas przyśpieszenia
    public float decelerationTime = 0.5f; // Czas opóźnienia
    private float currentRotationSpeed = 0f; // Bieżąca prędkość obrotu
    private float currentAccelerationTime = 0f; // Bieżący czas przyśpieszenia
    private float currentDecelerationTime = 0f; // Bieżący czas opóźnienia

    private int clickCount = 0; // Licznik kliknięć
    public float initialRotationOffset = 0f; // Początkowe przesunięcie rotacji


    private string id; // Dodaj pole przechowujące id

    [System.Serializable]
    public class DataResponse
    {
        public DataObject[] data;
        public AwardObject award;
    }

    [System.Serializable]
    public class DataObject
    {
        public string gold;
        public string diamond;
        public string chicken;
        public string ball;
        public string water;
    }

    [System.Serializable]
    public class AwardObject
    {
        public int diamond;
        public int ball;
        public int water;
        public int gold;
        public int chicken;
    }

    // Dodaj metodę SetId, która ustawia id
    public void SetId(string newId)
    {
        id = newId;
        Debug.Log("SET ID"+id);
    }

    public void SetLastDate(string date)
    {
        if (!string.IsNullOrEmpty(date))
        {
            long longValue = long.Parse(date);
            lastDate = longValue;
        }
        Debug.Log("SET date"+date);
    }



    void Start()
    {

        userId = ReferencesUserFirebase.userId;
        print("AA Click Dog Spot Player Id:" + userId);
        prevUserId = userId;
        // Dodaj losowe przesunięcie do początkowej rotacji
        transform.Rotate(Vector3.up, UnityEngine.Random.Range(0f, 360f));

        currentRotationSpeed = rotationSpeed; // Ustaw bieżącą prędkość na normalną prędkość obrotu
        dogspot.SetActive(true);

        gemPrefabs = new GameObject[5];
        gemPrefabs[0] = gemPrefab1;
        gemPrefabs[1] = gemPrefab2;
        gemPrefabs[2] = gemPrefab3;
        gemPrefabs[3] = gemPrefab4;
        gemPrefabs[4] = gemPrefab5;

        
        
    }

    // Update is called once per frame
    void Update()
    {
        // Pobierz obecną datę i godzinę w sekundach od epoki Unixa
        long currentTimestamp = GetCurrentTimestamp();

        // Sprawdź, czy lastDate nie jest równa zeru
        if (lastDate != 0)
        {
            // Oblicz różnicę między obecną datą a datą przechowywaną w lastDate
            long timeDifference = currentTimestamp - lastDate;

            // Sprawdź, czy minęło 24 godziny (86400 sekund)
            if (timeDifference <= 86400)
            {
                // Zmiana materiału dla wszystkich dzieci
                Renderer[] renderers = GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = visitedDogSpotMaterial;
                }
            }
        }
       
        if(prevUserId != ReferencesUserFirebase.userId)
        {
            userId = ReferencesUserFirebase.userId;
            prevUserId = userId;
        }
        // Obracaj z bieżącą prędkością obrotu
        transform.Rotate(Vector3.up, -currentRotationSpeed * Time.deltaTime);

        // Jeśli obiekt obraca się szybko
        if (currentRotationSpeed > rotationSpeed)
        {
            // Aktualizuj czas trwania szybkiego obrotu
            fastRotationDuration -= Time.deltaTime;

            // Jeśli czas minął, zacznij opóźnienie
            if (fastRotationDuration <= 0f)
            {
                currentDecelerationTime += Time.deltaTime;
                // Interpoluj prędkość obrotu z powrotem do normalnej prędkości
                currentRotationSpeed = Mathf.Lerp(fastRotationSpeedMultiplier * rotationSpeed, rotationSpeed, currentDecelerationTime / decelerationTime);
            }

            // Jeśli opóźnienie zakończyło się, wyłącz szybkie obracanie
            if (currentDecelerationTime >= decelerationTime)
            {
                currentRotationSpeed = rotationSpeed;
                fastRotationDuration = 1f;
                currentDecelerationTime = 0f;
            }
        }

        // Sprawdź, czy zostało kliknięte lub dotknięte
        if (Input.GetMouseButtonDown(0))
        {
             // Jeśli już kliknięto, zablokuj dalsze kliknięcia
            if (isClicked && clickedObject != dogspot)
            {
                return;
            }
            
            
            // Pobierz kliknięty obiekt
            if (dogspot == getClickedObject(out RaycastHit hit))
            {
                // Ustaw, że zaznaczyło dogspot aby uniemozliwić zaznaczanie innego
                isClicked = true;
                clickedObject = dogspot;

                if(!string.IsNullOrEmpty(id))
                {
                    Debug.Log("AAA DogSpot Id:"+id);
                }else{
                    Debug.Log("AAA Nie ma DogSpot Id:"+id);
                }
                clickCount++; // Inkrementuj licznik kliknięć

                if (clickCount == 3) // Jeśli kliknięto trzy razy
                {
                     StartCoroutine(SendRequest());
                     Debug.Log("AAA"+clickCount);
                // }
                //     print("3x kliknołeś"); // Wyświetl informację w konsoli

                    // Lista przechowująca pozycje już istniejących prefabów
                    List<Vector3> usedPositions = new List<Vector3>();

                    // Tworzenie gemów z wykorzystaniem tablicy prefabrykatów
                    GameObject dogSpotBack = GameObject.Find("DogSpotBack");
                    // for (int i = 0; i < 5; i++)
                    // {
                    //     Vector3 gemPosition = GenerateRandomPosition(usedPositions);

                    //     // Utwórz obiekt gem w losowej pozycji
                    //     GameObject gem = Instantiate(gemPrefabs[i], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                    //     gem.name = "Gem"; // Nadaj nazwę elementowi
                    //     gem.transform.localScale = new Vector3(30f, 30f, 30f);
                    //     gem.layer = LayerMask.NameToLayer("UI");
                    //     gem.transform.SetParent(dogSpotBack.transform, false);

                    //     // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                    //     usedPositions.Add(gemPosition);
                    // }

                     clickCount = 0; // Zresetuj licznik kliknięć
                }

                currentRotationSpeed = fastRotationSpeedMultiplier * rotationSpeed; // Ustaw prędkość na szybką prędkość obrotu
                currentDecelerationTime = 0f; // Zresetuj czas opóźnienia

                virtualCamera.LookAt = dogspot.transform;
                // Ustaw priorytet kamery na 50
                virtualCamera.Priority = 50;

                // Wyłącz menu
                if (menuToDisable != null)
                {
                    menuToDisable.SetActive(false);
                    menuToEnable.SetActive(true);
                }
            }
        }
    }

    public static void ResetClick()
    {
        // Debug.Log("RESET CLICK");
        isClicked = false;
        clickedObject = null;
    }

    IEnumerator SendRequest()
    {
        
        string url = $"https://psiaapka.pl/visitdogspot.php?dog_spot_id={id}&user_id={userId}&message=test";
        Debug.Log("AAA SendRequest: "+ url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            int dogSpotId = int.Parse(id);
            AddVisitedDogSpot(dogSpotId);

            // Zmiana materiału dla wszystkich dzieci
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material = visitedDogSpotMaterial;
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
              
                string responseText = request.downloadHandler.text;
           
                // Sprawdź, czy odpowiedź to JSON
                if (responseText.StartsWith("{"))
                {
                    DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(responseText);
                    // Odpowiedź JSON
                  
                    GlobalData.Instance.UpdateData(int.Parse(dataResponse.data[0].gold), int.Parse(dataResponse.data[0].diamond), int.Parse(dataResponse.data[0].chicken), int.Parse(dataResponse.data[0].ball), int.Parse(dataResponse.data[0].water));
                    
                    int awardGold = dataResponse.award.gold;
                    int awardDiamond = dataResponse.award.diamond;
                    int awardBall = dataResponse.award.ball;
                    int awardWater = dataResponse.award.water;
                    int awardChicken = dataResponse.award.chicken;

                    // Lista przechowująca pozycje już istniejących prefabów
                    List<Vector3> usedPositions = new List<Vector3>();

                    // Tworzenie gemów z wykorzystaniem tablicy prefabrykatów
                    GameObject dogSpotBack = GameObject.Find("DogSpotBack");

                    // Wykonaj kod tworzenia obiektów gem tylko jeśli otrzymano odpowiedź JSON
                   if(awardGold > 0)
                   {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);

                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[1], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Gold"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                   }  

                   if(awardDiamond > 0)
                   {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);

                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[2], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Gem"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                   }     

                   if(awardWater > 0)
                   {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);

                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[3], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Water"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                   }     

                   if(awardBall > 0)
                   {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);

                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[4], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Ball"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                   }     

                   if(awardChicken > 0)
                   {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);

                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[5], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Chicken"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                   }     
                    
                }
                else
                {
                    // Odpowiedź nie jest JSON-em, nie wykonuj dodatkowych działań
                    Debug.Log("AAA Odpowiedź nie jest w formacie JSON: " + responseText);
                    // Sprawdź, czy istnieje canvas o nazwie "Noticeboard" jako dziecko tego obiektu
                    Transform noticeboard = transform.Find("Noticeboard");
                    
                    // Jeśli canvas został znaleziony, ustaw go jako aktywny
                    if (noticeboard != null)
                    {
                        noticeboard.gameObject.SetActive(true);
                    }
                    else
                    {
                        Debug.LogError("Canvas 'Noticeboard' not found as a child of this object.");
                    }
                }
            }
            else
            {
                // Obsłuż błąd
                Debug.LogError("AAA Błąd podczas wysyłania zapytania: " + request.error);
            }
        }
    }

    public void CloseNoticeBoard()
    {
         // Znajdź wszystkie canvasy o nazwie "Noticeboard" na planszy
            Canvas[] noticeboards = FindObjectsOfType<Canvas>();

            // Przejdź przez wszystkie znalezione canvasy
            foreach (Canvas noticeboard in noticeboards)
            {
                // Sprawdź, czy canvas ma nazwę "Noticeboard"
                if (noticeboard.gameObject.name == "Noticeboard")
                {
                    // Ustaw canvas na nieaktywny
                    noticeboard.gameObject.SetActive(false);
                }
            }
    }

    Vector3 GenerateRandomPosition(List<Vector3> usedPositions)
    {
        // Dopóki nie zostanie znaleziona odpowiednia pozycja, kontynuuj próby generowania
        while (true)
        {
            float randomX = UnityEngine.Random.Range(-Screen.width / 4f, Screen.width / 4f); // Bliżej środka ekranu w osi X
            float randomY = UnityEngine.Random.Range(-Screen.height / 4f, Screen.height / 4f); // Bliżej środka ekranu w osi Y
            Vector3 gemPosition = new Vector3(randomX, randomY, 0f);

            // Sprawdź, czy nowa pozycja jest wystarczająco oddalona od pozycji istniejących prefabów
            bool isValidPosition = true;
            foreach (Vector3 usedPosition in usedPositions)
            {
                float distance = Vector3.Distance(usedPosition, gemPosition);
                if (distance < 50f) // Dystans, który musi być zachowany, aby uniknąć nachodzenia na siebie prefabów
                {
                    isValidPosition = false;
                    break;
                }
            }

            // Jeśli nowa pozycja jest wystarczająco oddalona od pozycji istniejących prefabów, zwróć ją
            if (isValidPosition)
            {
                return gemPosition;
            }
        }
    }

    GameObject getClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (!isPointerOverUIObject()) { target = hit.collider.gameObject; }
        }

        return target;
    }

    public void UsunElementPoNazwie(GameObject dogSpotBack)
    {
        string nazwaElementu = "Gem";
        string chickenName  = "Chicken";
        string goldName  = "Gold";
        string waterName  = "Water";
        string ballName  = "Ball";
        // Sprawdź, czy obiekt dogSpotBack istnieje
        if (dogSpotBack != null)
        {
            // Pętla szukająca dzieci obiektu dogSpotBack o danej nazwie i usuwająca je
            foreach (Transform child in dogSpotBack.transform)
            {
                if (child.gameObject.name == nazwaElementu)
                {
                    Destroy(child.gameObject);
                }
                if (child.gameObject.name == chickenName)
                {
                    Destroy(child.gameObject);
                }
                 if (child.gameObject.name == goldName)
                {
                    Destroy(child.gameObject);
                }
                 if (child.gameObject.name == waterName)
                {
                    Destroy(child.gameObject);
                }
                 if (child.gameObject.name == ballName)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        else
        {
            Debug.LogError("Obiekt dogSpotBack nie istnieje lub nie został przekazany do funkcji UsunElementPoNazwie.");
        }
    }

    private bool isPointerOverUIObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }
}
