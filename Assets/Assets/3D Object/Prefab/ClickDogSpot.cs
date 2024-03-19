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

    public float rotationSpeed = 50f; // Prędkość obrotu
    public float fastRotationSpeedMultiplier = 2f; // Mnożnik szybkości obrotu po kliknięciu
    public float fastRotationDuration = 1f; // Czas trwania szybkiego obrotu po kliknięciu
    public float accelerationTime = 0.5f; // Czas przyśpieszenia
    public float decelerationTime = 0.5f; // Czas opóźnienia
    private float currentRotationSpeed = 0f; // Bieżąca prędkość obrotu
    private float currentAccelerationTime = 0f; // Bieżący czas przyśpieszenia
    private float currentDecelerationTime = 0f; // Bieżący czas opóźnienia



    public float initialRotationOffset = 0f; // Początkowe przesunięcie rotacji

    void Start()
    {
        // Dodaj losowe przesunięcie do początkowej rotacji
        transform.Rotate(Vector3.up, Random.Range(0f, 360f));
        currentRotationSpeed = rotationSpeed; // Ustaw bieżącą prędkość na normalną prędkość obrotu
        dogspot.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Obracaj z bieżącą prędkością obrotu
        transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);

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
                print("Clicked and touch");
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

    private bool isPointerOverUIObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }
}
