using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI messageText; // Referencja do komponentu TextMeshPro
    public GameObject messagePanel; // Panel zawierający TextMeshPro, który będzie ukrywany
    private bool messageDisplayed = false; // Flaga kontrolująca, czy komunikat został już wyświetlony

    public Slider secondSlider; // Dodanie drugiego suwak

    private int progress = 0;

    void Start()
    {
        messagePanel.SetActive(false); // Ukryj panel na początku gry
    }

    public void OnSliderChanged(float value)
    {
        // Możesz dodać tu kod do aktualizacji interfejsu w czasie rzeczywistym
    }

    public void UpdateProgress()
    {
        progress++;
        slider.value = progress;
        Debug.Log("progress " + progress);
        float percentage = slider.value / slider.maxValue;
        if (percentage >= 1.0f && !messageDisplayed)
        {
            if (Mathf.Approximately(secondSlider.value, secondSlider.maxValue))
            {
                Invoke("ShowMessage", 3f);
                messageDisplayed = true; // Ustaw flagę na true po wyświetleniu komunikatu
            }
             
        }
    }

    private void ShowMessage()
    {
        Taptic.Default();
        string randomMessage = HappyDogMessages.GetRandomMessage();
        messageText.text = randomMessage;
        messagePanel.SetActive(true);
    }
}


public class HappyDogMessages
{
    private static readonly string[] messages = new string[]
    {
       "Gratulacje! Twój pies jest teraz pełen energii i zadowolenia!",
        "Brawo! Twój pies jest najedzony i szczęśliwy!",
        "Świetna robota! Twój pies ma pełny brzuszek i jest radosny!",
        "Fantastycznie! Twój pies cieszy się z pysznego posiłku!",
        "Doskonale! Twój pies ma teraz pełną miskę i jest bardzo zadowolony!",
        "Niesamowite! Twój pies jest najedzony i wypoczęty!",
        "Cudownie! Twój pies osiągnął pełnię sytości i szczęścia!",
        "Wspaniale! Twój pies pije wodę z radością i jest usatysfakcjonowany!",
        "Rewelacja! Twój pies ma teraz pełny brzuszek i jest zachwycony!",
        "Super! Twój pies jest teraz dobrze nawodniony i zadowolony!",
        "Kapitalnie! Twój pies nigdy wcześniej nie był tak najedzony i szczęśliwy!",
        "Hurra! Twój pies ma pełną misę i jest teraz w pełni usatysfakcjonowany!",
        "Znakomicie! Twój pies pije wodę z radością i jest przepełniony szczęściem!",
        "Niesamowite! Twój pies jest teraz w pełni syty i szczęśliwy!",
        "Wow! Twój pies nie mógłby być bardziej usatysfakcjonowany po posiłku!",
        "Super posiłek! Twój pies jest teraz mega najedzony i szczęśliwy!",
        "Gratulacje! Twój pies osiągnął pełnię sytości i radości!",
        "Rewelacyjnie! Twój pies jest teraz w pełni najedzony i zrelaksowany!",
        "Fantastycznie! Twój pies bawił się na pełnych brzuszkach!",
        "Świetna robota! Twój pies jest teraz pełen energii i zadowolenia po posiłku!"
    };

    private static readonly System.Random random = new System.Random();

    public static string GetRandomMessage()
    {
        int index = random.Next(messages.Length);
        return messages[index];
    }
}