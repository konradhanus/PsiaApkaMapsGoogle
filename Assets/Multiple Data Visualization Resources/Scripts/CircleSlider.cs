using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    public bool b = true;
    public Image image;
    public float speed = 0.5f;

    private float time = 0f;
    public Text progress;

    // Nowa zmienna do przechowywania liczby kroków
    private float steps = 0f; // Przykładowa wartość kroków
    public const float MAX_STEPS = 12000f; // Maksymalna wartość kroków
    private float previousFillAmount = -1f; // Przechowuje poprzednią wartość fillAmount
    private string previousProgressText = ""; // Przechowuje poprzedni tekst progress

    void Update()
    {
        // Sprawdź, czy tekst w progress uległ zmianie
        if (progress != null && progress.text != previousProgressText)
        {
            previousProgressText = progress.text; // Zaktualizuj poprzedni tekst

            // Sparsuj nową wartość
            if (float.TryParse(previousProgressText, out float parsedSteps))
            {
                steps = parsedSteps; // Przypisz wartość kroków
            }
            else
            {
                Debug.LogError("Nie udało się sparsować tekstu na float.");
                return; // Zakończ aktualizację, jeśli nie udało się sparsować
            }

            // Oblicz fillAmount jako iloraz aktualnych kroków do maksymalnej wartości
            float value = Mathf.Clamp(steps / MAX_STEPS, 0f, 1f);
            Debug.Log("steps: " + steps + " value: " + value);

            // Aktualizuj tylko wtedy, gdy wartość fillAmount zmieniła się
            if (Mathf.Abs(image.fillAmount - value) > 0.001f)
            {
                image.fillAmount = value; // Ustaw nową wartość tylko jeśli się zmieniła
                previousFillAmount = value; // Zaktualizuj poprzednią wartość fillAmount
            }
        }

        if (progress)
        {
            // Można odkomentować i aktualizować procentowy wynik, jeśli potrzebne
            // progress.text = ((int)(image.fillAmount * 100)).ToString() + "%"; 
        }
    }
}
