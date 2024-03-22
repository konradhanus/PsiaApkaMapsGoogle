using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickDogSpot : MonoBehaviour
{
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

    void Start()
    {
        // Dodaj losowe przesunięcie do początkowej rotacji
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
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
            // Pobierz kliknięty obiekt
            if (dogspot == getClickedObject(out RaycastHit hit))
            {
                clickCount++; // Inkrementuj licznik kliknięć

                if (clickCount == 3) // Jeśli kliknięto trzy razy
                {
                    print("3x kliknołeś"); // Wyświetl informację w konsoli

                    // Lista przechowująca pozycje już istniejących prefabów
                    List<Vector3> usedPositions = new List<Vector3>();

                    // Tworzenie gemów z wykorzystaniem tablicy prefabrykatów
                    GameObject dogSpotBack = GameObject.Find("DogSpotBack");
                    for (int i = 0; i < 5; i++)
                    {
                        Vector3 gemPosition = GenerateRandomPosition(usedPositions);

                        // Utwórz obiekt gem w losowej pozycji
                        GameObject gem = Instantiate(gemPrefabs[i], gemPosition, Quaternion.Euler(-90f, 0f, 0f));
                        gem.name = "Gem"; // Nadaj nazwę elementowi
                        gem.transform.localScale = new Vector3(30f, 30f, 30f);
                        gem.layer = LayerMask.NameToLayer("UI");
                        gem.transform.SetParent(dogSpotBack.transform, false);

                        // Dodaj pozycję nowo utworzonego obiektu do listy użytych pozycji
                        usedPositions.Add(gemPosition);
                    }

                    // clickCount = 0; // Zresetuj licznik kliknięć
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

    Vector3 GenerateRandomPosition(List<Vector3> usedPositions)
    {
        // Dopóki nie zostanie znaleziona odpowiednia pozycja, kontynuuj próby generowania
        while (true)
        {
            float randomX = Random.Range(-Screen.width / 4f, Screen.width / 4f); // Bliżej środka ekranu w osi X
            float randomY = Random.Range(-Screen.height / 4f, Screen.height / 4f); // Bliżej środka ekranu w osi Y
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
