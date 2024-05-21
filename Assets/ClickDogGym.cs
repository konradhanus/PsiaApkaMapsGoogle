using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ClickDogGym : MonoBehaviour
{
    private int clickCount = 0; // Licznik kliknięć
    private const int maxClicks = 3; // Maksymalna liczba kliknięć do zmiany sceny
    public GameObject menuToDisable; // Menu do wyłączenia
    public GameObject menuToEnable; // Menu do włączenia

    private void Start()
    {
        clickCount = 0; // Zainicjuj licznik kliknięć
    }

    private void Update()
    {
        // Sprawdź, czy zostało kliknięte lub dotknięte
        if (Input.GetMouseButtonDown(0))
        {
            // Pobierz kliknięty obiekt
            GameObject clickedObject = GetClickedObject(out RaycastHit hit);

            // Sprawdź, czy kliknięty obiekt to ten skrypt
            if (clickedObject == gameObject)
            {
                clickCount++; // Inkrementuj licznik kliknięć

                if (clickCount == maxClicks)
                {
                    // Przenieś na scenę nr 4
                    SceneManager.LoadSceneAsync(1);
                }

                // Wyłącz menu
                if (menuToDisable != null)
                {
                    menuToDisable.SetActive(false);
                }

                // Włącz menu
                if (menuToEnable != null)
                {
                    menuToEnable.SetActive(true);
                }
            }
        }
    }

    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (!IsPointerOverUIObject())
            {
                target = hit.collider.gameObject;
            }
        }
        return target;
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
