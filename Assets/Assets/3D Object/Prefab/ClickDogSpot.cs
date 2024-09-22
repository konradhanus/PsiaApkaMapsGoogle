using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class ClickDogSpot : MonoBehaviour
{


    private Coonsole console;

    public bool DebugLog = false;

    private static bool isClicked = false; // Statyczna zmienna, aby przechowywać stan kliknięcia
    private static GameObject clickedObject = null; // Statyczna zmienna do przechowywania obiektu, który zablokował kliknięcia

    private string userId;
    private long lastDate;
    private string prevUserId;
    public GameObject dogspot;
    public GameObject PlayerArmature;
    public bool isDogGym = false;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    public GameObject menuToDisable; // Menu do wyłączenia
    public GameObject menuToEnable; // Menu do wyłączenia
    public GameObject gemPrefab1; // Prefabrykat obiektu gem
    public GameObject gemPrefab2; // Prefabrykat obiektu gem
    public GameObject gemPrefab3; // Prefabrykat obiektu gem
    public GameObject gemPrefab4; // Prefabrykat obiektu gem
    public GameObject gemPrefab5; // Prefabrykat obiektu gem
    public GameObject[] gemPrefabs;
    public bool hasVisited = false;
    public string dateVisited = "null";

    // SAVE DATA
    private const string VisitedDogSpotId = "VisitedDogSpotId";

    private void SaveVisitedDogSpots()
    {
        long lastVisitTimestamp = GetCurrentTimestamp();
        string data = lastVisitTimestamp.ToString();
        string keyDogSpot = VisitedDogSpotId + id;
        PlayerPrefs.SetString(keyDogSpot, data);
        PlayerPrefs.Save();
        if (DebugLog) Debug.Log("ClickDogSpot: SAVE: " + data);
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
        Debug.Log("ClickDogSpot: SET ID" + id);
    }

    // Dodaj metodę SetId, która ustawia id
    public void SetVisited()
    {
        hasVisited = true;
        // Zmiana materiału dla wszystkich dzieci
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = visitedDogSpotMaterial;
        }
        Debug.Log("ClickDogSpot: SET VISITED");
    }

    public void SetLastDate(string date)
    {
        if (!string.IsNullOrEmpty(date))
        {
            long longValue = long.Parse(date);
            lastDate = longValue;
        }
        if (DebugLog) Debug.Log("ClickDogSpot: SET date" + date);
    }


    void Start()
    {
        console = new Coonsole("ClickDogSpot");

        // Sprawdź, czy userId jest ustawione w ReferencesUserFirebase
        userId = ReferencesUserFirebase.userId;

        if (string.IsNullOrEmpty(userId))
        {
            console.Warn("ClickDogSpot: User ID is not set or is empty set userId: test !");
            // Możesz dodać domyślne ID lub zakończyć metodę, jeśli userId jest wymagane
            userId = "test";
            //return; // Zakończ wykonanie, jeśli userId jest puste
        }

        if (DebugLog) Debug.Log("ClickDogSpot: AA Click Dog Spot Player Id: " + userId);
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

        if (prevUserId != ReferencesUserFirebase.userId)
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
                CloseInfoBar();
                Transform infoBar = transform.Find("InfoBar");

                // Ustaw, że zaznaczyło dogspot aby uniemozliwić zaznaczanie innego
                isClicked = true;
                clickedObject = dogspot;

                StartCoroutine(HideOtherDogSpotsAndGymsWithDelay(clickedObject, 0.4f));

                // 

                if (!string.IsNullOrEmpty(id))
                {
                    //Debug.Log("AAA DogSpot Id:"+id);
                }
                else
                {
                    //Debug.Log("AAA Nie ma DogSpot Id:"+id);
                }
                clickCount++; // Inkrementuj licznik kliknięć

                if (DebugLog) Debug.Log("ClickDogSpot: ClickDogSpot distance: " + Vector3.Distance(PlayerArmature.transform.position, transform.position));

                if (Vector3.Distance(PlayerArmature.transform.position, transform.position) > 55f)
                {


                    if (infoBar != null)
                    {
                        infoBar.gameObject.SetActive(true);
                    }

                    if (DebugLog) Debug.Log("ClickDogSpot: za daleko" + Vector3.Distance(PlayerArmature.transform.position, transform.position));
                }
                else
                {
                    // Debug.Log("za blisko" + Vector3.Distance(PlayerArmature.transform.position, transform.position));
                    if (infoBar != null)
                    {
                        infoBar.gameObject.SetActive(false);
                    }
                    if (clickCount == 3) // Jeśli kliknięto trzy razy
                    {
                        if (DebugLog) Debug.Log("ClickDogSpot: Jeśli kliknięto trzy razy");

                        if (!isDogGym)
                        {
                            console.Log("test");
                            StartCoroutine(SendRequest());
                        }
                        else
                        {
                            console.Log("test2");
                            SceneManager.LoadSceneAsync(3);
                        }
                        //Debug.Log("AAA"+clickCount);
                        // }
                        if (DebugLog) Debug.Log("ClickDogSpot: 3x kliknołeś"); // Wyświetl informację w konsoli

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

                }

                currentRotationSpeed = fastRotationSpeedMultiplier * rotationSpeed; // Ustaw prędkość na szybką prędkość obrotu
                currentDecelerationTime = 0f; // Zresetuj czas opóźnienia

                virtualCamera.LookAt = dogspot.transform;
                // Ustaw priorytet kamery na 50
                virtualCamera.Priority = 50;

                // Pobranie aktualnej pozycji dogspot
                Vector3 dogspotPosition = dogspot.transform.position;
                Vector3 virutalCameraPosition = virtualCamera.transform.position;

                // Dodanie przesunięcia o x: 20, y: 10
                //Vector3 cameraPosition = new Vector3(dogspotPosition.x + 3, 2, dogspotPosition.z + 3);

                Vector3 cameraPosition = new Vector3(dogspotPosition.x, 50, dogspotPosition.z + 50);

                // Ustawienie nowej pozycji kamery
                virtualCamera.transform.position = cameraPosition;

                // Wyłącz menu
                if (menuToDisable != null)
                {
                    menuToDisable.SetActive(false);
                    menuToEnable.SetActive(true);
                }
            }
        }
    }

    public static List<GameObject> FindGameObjectsWithTagIncludingInactive(string tag)
    {
        List<GameObject> results = new List<GameObject>();
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && obj.hideFlags == HideFlags.None)
            {
                results.Add(obj);
            }
        }

        return results;
    }

    public static void ResetClick()
    {
        Debug.Log("RESET CLICK");
        isClicked = false;
        clickedObject = null;

        GameObject[] noticeboards = GameObject.FindGameObjectsWithTag("Noticeboard");

        // Ustaw każdy znaleziony obiekt jako nieaktywny
        foreach (GameObject noticeboard in noticeboards)
        {
            noticeboard.SetActive(false);
        }

    }

    private IEnumerator HideOtherDogSpotsAndGymsWithDelay(GameObject clickedPoint, float delay)
    {
        // Czekaj przez określony czas
        yield return new WaitForSeconds(delay);

        // Znajdź wszystkie obiekty z tagiem DogSpot
        GameObject[] dogSpots = GameObject.FindGameObjectsWithTag("DogSpot");
        GameObject[] dogGyms = GameObject.FindGameObjectsWithTag("DogGym");

        // Ukryj wszystkie obiekty z tagiem DogSpot, które nie są klikniętym obiektem
        foreach (GameObject dogSpot in dogSpots)
        {
            if (dogSpot != clickedPoint)
            {
                dogSpot.SetActive(false);
            }
        }

        // Ukryj wszystkie obiekty z tagiem DogGym
        foreach (GameObject dogGym in dogGyms)
        {
            if (dogGym != clickedPoint)
            {
                dogGym.SetActive(false);
            }
        }
    }

    public void ShowAllDogSpotsAndGyms()
    {
        StartCoroutine(ShowAllWithDelay());

        if (DebugLog) Debug.Log("ClickDogSpot: SHOW ALL");
    }


    private IEnumerator ShowAllWithDelay()
    {
        // Znajdź wszystkie obiekty z tagiem DogSpot
        List<GameObject> dogSpots = GameObjectFinder.FindGameObjectsWithTagIncludingInactive("DogSpot");
        List<GameObject> dogGyms = GameObjectFinder.FindGameObjectsWithTagIncludingInactive("DogGym");

        // Pokaż wszystkie obiekty z tagiem DogSpot
        foreach (GameObject dogSpot in dogSpots)
        {
            dogSpot.SetActive(true);
        }

        // Pokaż wszystkie obiekty z tagiem DogGym
        foreach (GameObject dogGym in dogGyms)
        {
            dogGym.SetActive(true);
        }

        if (DebugLog) Debug.Log("ClickDogSpot: SHOW ALL");

        // Odczekaj 0,7 sekundy
        yield return new WaitForSeconds(0.2f);

        // Ponowne uruchomienie tej samej logiki
        foreach (GameObject dogSpot in dogSpots)
        {
            dogSpot.SetActive(true);
        }

        foreach (GameObject dogGym in dogGyms)
        {
            dogGym.SetActive(true);
        }

        if (DebugLog) Debug.Log("ClickDogSpot: SHOW ALL AGAIN");

        if (DebugLog) Debug.Log("ClickDogSpot: SHOW ALL");

        // Odczekaj 0,7 sekundy
        yield return new WaitForSeconds(0.4f);

        // Ponowne uruchomienie tej samej logiki
        foreach (GameObject dogSpot in dogSpots)
        {
            dogSpot.SetActive(true);
        }

        foreach (GameObject dogGym in dogGyms)
        {
            dogGym.SetActive(true);
        }

        if (DebugLog) Debug.Log("ClickDogSpot: SHOW ALL AGAIN");

        if (DebugLog) Debug.Log("ClickDogSpot: SHOW ALL");

        // Odczekaj 0,7 sekundy
        yield return new WaitForSeconds(0.6f);

        // Ponowne uruchomienie tej samej logiki
        foreach (GameObject dogSpot in dogSpots)
        {
            dogSpot.SetActive(true);
        }

        foreach (GameObject dogGym in dogGyms)
        {
            dogGym.SetActive(true);
        }

        if (DebugLog) Debug.Log("ClickDogSpot: SHOW ALL AGAIN");


    }


    IEnumerator SendRequest()
    {
        console.Log("tu");
       
        console.Log("tu2");
        //Debug.Log("AAA SendRequest: "+ url);

        // Sprawdź, czy id i userId są poprawne
        if (string.IsNullOrEmpty(id))
        {
            console.Error("ID cannot be null or empty.");
            yield break; // Zakończ IEnumerator, jeśli id jest niepoprawne
        }

        if (string.IsNullOrEmpty(userId))
        {
            console.Warn("User ID cannot be null or empty. Set in SendRequest: userId = test");
            userId = "test";
            //yield break; // Zakończ IEnumerator, jeśli userId jest niepoprawne
        }

        string url = $"https://psiaapka.pl/visitdogspot.php?dog_spot_id={id}&user_id={userId}&message=test";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            console.Log("tu3");

            int dogSpotId = int.Parse(id);
            console.Log("tu4");
            AddVisitedDogSpot(dogSpotId);
            console.Log("tu5");

            // Zmiana materiału dla wszystkich dzieci
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            console.Log("tu6");
            foreach (Renderer renderer in renderers)
            {
                renderer.material = visitedDogSpotMaterial;
                console.Log("tu7");
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                console.Log("tu8");

                string responseText = request.downloadHandler.text;
                //Debug.Log("AAA SendRequest: response "+ responseText);
                // Sprawdź, czy odpowiedź to JSON
                if (responseText.StartsWith("{"))
                {
                    //Debug.Log("AAA SendRequest: response jestem w ");
                    DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(responseText);
                    //Debug.Log("AAA SendRequest: ZA FORMJSON ");
                    // Odpowiedź JSON

                    //GlobalData.Instance.UpdateData(int.Parse(dataResponse.data[0].gold), int.Parse(dataResponse.data[0].diamond), int.Parse(dataResponse.data[0].chicken), int.Parse(dataResponse.data[0].ball), int.Parse(dataResponse.data[0].water));
                    //Debug.Log("AAA SendRequest: ZA GlobalData ");
                    int awardGold = dataResponse.award.gold;
                    int awardDiamond = dataResponse.award.diamond;
                    int awardBall = dataResponse.award.ball;
                    int awardWater = dataResponse.award.water;
                    int awardChicken = dataResponse.award.chicken;
                    // Debug.Log("AAA SendRequest: ZA awardChicken ");
                    // Lista przechowująca pozycje już istniejących prefabów
                    List<Vector3> usedPositions = new List<Vector3>();

                    // Tworzenie gemów z wykorzystaniem tablicy prefabrykatów
                    GameObject dogSpotBack = GameObject.Find("DogSpotBack");
                    //Debug.Log("AAA SendRequest: ZA GameObject ");
                    // Wykonaj kod tworzenia obiektów gem tylko jeśli otrzymano odpowiedź JSON
                    if (awardGold > 0)
                    {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);
                        //Debug.Log("AAA SendRequest: GOLD ");
                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[1], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Gold"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                    }

                    if (awardDiamond > 0)
                    {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);
                        //Debug.Log("AAA SendRequest: DIAMOND ");
                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[2], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Gem"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                    }

                    if (awardWater > 0)
                    {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);
                        //Debug.Log("AAA SendRequest: WATER ");
                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[3], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Water"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                    }

                    if (awardBall > 0)
                    {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);
                        //Debug.Log("AAA SendRequest: BALL ");
                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[4], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Ball"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                    }

                    if (awardChicken > 0)
                    {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);
                        //Debug.Log("AAA SendRequest: CHICKEN ");
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
                    //Debug.Log("AAA SendRequest: ELSE ");
                    // Odpowiedź nie jest JSON-em, nie wykonuj dodatkowych działań
                    //Debug.Log("AAA Odpowiedź nie jest w formacie JSON: " + responseText);
                    // Sprawdź, czy istnieje canvas o nazwie "Noticeboard" jako dziecko tego obiektu
                    Transform noticeboard = transform.Find("Noticeboard");

                    // Jeśli canvas został znaleziony, ustaw go jako aktywny
                    if (noticeboard != null)
                    {
                        noticeboard.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (DebugLog) Debug.LogError("Canvas 'Noticeboard' not found as a child of this object.");
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

    public void CloseInfoBar()
    {
        // Znajdź wszystkie canvasy o nazwie "Noticeboard" na planszy
        Canvas[] infobars = FindObjectsOfType<Canvas>();

        // Przejdź przez wszystkie znalezione canvasy
        foreach (Canvas infobar in infobars)
        {
            // Sprawdź, czy canvas ma nazwę "Noticeboard"
            if (infobar.gameObject.name == "InfoBar")
            {
                // Ustaw canvas na nieaktywny
                infobar.gameObject.SetActive(false);
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
        string chickenName = "Chicken";
        string goldName = "Gold";
        string waterName = "Water";
        string ballName = "Ball";
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


public static class GameObjectFinder
{
    public static List<GameObject> FindGameObjectsWithTagIncludingInactive(string tag)
    {
        List<GameObject> results = new List<GameObject>();
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag) && obj.hideFlags == HideFlags.None)
            {
                results.Add(obj);
            }
        }

        return results;
    }
}