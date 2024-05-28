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
        progress++;
        slider.value = progress;
        Debug.Log("progress " + progress);

        if (progress >= 100 && !messageDisplayed)
        {
            ShowMessage();
            messageDisplayed = true; // Ustaw flagę na true po wyświetleniu komunikatu
        }
    }

    private void ShowMessage()
    {
        string randomMessage = HappyDogMessages.GetRandomMessage();
        messageText.text = randomMessage;
        messagePanel.SetActive(true);
    }
}


public class HappyDogMessages
{
    private static readonly string[] messages = new string[]
    {
        "Gratulacje! Twój pies jest super szczęśliwy!",
        "Brawo! Twój pies jest teraz najszczęśliwszy na świecie!",
        "Świetna robota! Twój pies jest pełen radości!",
        "Fantastycznie! Twój pies bawił się znakomicie!",
        "Doskonale! Twój pies nie mógłby być szczęśliwszy!",
        "Niesamowite! Twój pies jest w siódmym niebie!",
        "Cudownie! Twój pies osiągnął maksymalny poziom radości!",
        "Wspaniale! Twój pies jest teraz pełen energii i szczęścia!",
        "Rewelacja! Twój pies bawił się jak nigdy wcześniej!",
        "Super! Twój pies jest teraz absolutnie zachwycony!",
        "Kapitalnie! Twój pies nigdy wcześniej nie był tak szczęśliwy!",
        "Hurra! Twój pies jest teraz w pełni szczęśliwy!",
        "Znakomicie! Twój pies bawił się świetnie!",
        "Niesamowita zabawa! Twój pies jest pełen radości!",
        "Wow! Twój pies nie mógłby być bardziej zadowolony!",
        "Super zabawa! Twój pies jest teraz mega szczęśliwy!",
        "Gratulacje! Twój pies osiągnął pełnię szczęścia!",
        "Rewelacyjnie! Twój pies jest w ekstazie!",
        "Fantastycznie! Twój pies bawił się na całego!",
        "Świetna zabawa! Twój pies jest przepełniony radością!",
        "Kapitalna robota! Twój pies jest teraz najszczęśliwszy na świecie!"
    };

    private static readonly System.Random random = new System.Random();

    public static string GetRandomMessage()
    {
        int index = random.Next(messages.Length);
        return messages[index];
    }
}