using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarChart : MonoBehaviour
{
    public Color barColor = new Color(0.2f, 0.6f, 1f, 1f); // Niebieski kolor słupków
    public float barWidth = 50f; // Szerokość słupków
    public float maxHeight = 300f; // Maksymalna wysokość słupka
    public float spacing = 10f; // Odstęp między słupkami
    private GameObject chartContainer;

    // Start to initialize everything
    void Start()
    {
        // Tworzenie kontenera na wykres (tło panelu) jako dziecko obiektu, do którego jest przypisany ten skrypt
        chartContainer = CreatePanel(new Vector2(800, 500), new Vector2(-200, -275), Color.white);

        // Ustawienie kontenera jako dziecko obiektu, do którego jest przypisany ten skrypt
        chartContainer.transform.SetParent(transform, false);

        // Ustawienie skali 0.5 dla kontenera wykresu
        chartContainer.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        // Przykład z danymi - kroki z ostatnich 7 dni
        DrawBarChart(5000, 8000, 7500, 6000, 9200, 4500, 10000);
    }

    // Funkcja rysująca wykres
    public void DrawBarChart(int day1ago, int day2ago, int day3ago, int day4ago, int day5ago, int day6ago, int day7ago)
    {
        int[] stepsData = new int[] { day1ago, day2ago, day3ago, day4ago, day5ago, day6ago, day7ago };

        // Znalezienie maksymalnej wartości do przeskalowania słupków
        int maxSteps = Mathf.Max(stepsData);

        for (int i = 0; i < stepsData.Length; i++)
        {
            // Przeskalowanie wysokości słupka
            float normalizedHeight = (float)stepsData[i] / maxSteps;
            float barHeight = normalizedHeight * maxHeight;

            // Tworzenie i ustawianie słupka jako dziecko chartContainer
            GameObject bar = CreateBar(new Vector2(barWidth, barHeight), new Vector2(i * (barWidth + spacing), 0), barColor);
            bar.transform.SetParent(chartContainer.transform, false);
        }
    }

    // Funkcja tworząca panel (kontener na wykres)
    private GameObject CreatePanel(Vector2 size, Vector2 position, Color color)
    {
        GameObject panel = new GameObject("ChartPanel");
        RectTransform rt = panel.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;

        // Dodaj komponent Image i ustaw kolor
        Image img = panel.AddComponent<Image>();
        img.color = color;

        return panel;
    }

    // Funkcja tworząca pojedynczy słupek
    private GameObject CreateBar(Vector2 size, Vector2 position, Color color)
    {
        GameObject bar = new GameObject("Bar");
        RectTransform rt = bar.AddComponent<RectTransform>();
        rt.sizeDelta = size;
        rt.anchoredPosition = position;

        // Dodaj komponent Image i ustaw kolor
        Image img = bar.AddComponent<Image>();
        img.color = color;

        return bar;
    }
}
