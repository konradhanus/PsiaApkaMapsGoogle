using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public static GlobalData Instance;

    // Dane z API
    public int gold = 6;
    public int diamond = 6;
    public int chicken = 6;
    public int ball = 6;
    public int water = 6;

    void Awake()
    {
        // Sprawdź, czy istnieje już instancja tego obiektu
        if (Instance == null)
        {
            // Jeśli nie, ustaw ten obiekt jako instancję
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jeśli instancja już istnieje, zniszcz ten obiekt, aby uniknąć duplikatów
            Destroy(gameObject);
        }
    }

    // Metoda do aktualizacji danych z odpowiedzi API
    public void UpdateData(int newGold, int newDiamond, int newChicken, int newBall, int newWater)
    {
        gold = newGold;
        diamond = newDiamond;
        chicken = newChicken;
        ball = newBall;
        water = newWater;
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
