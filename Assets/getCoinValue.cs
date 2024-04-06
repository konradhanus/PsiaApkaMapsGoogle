using UnityEngine;
using TMPro;

public class GetCoinValue : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    void OnEnable()
    {
        // Deklaruj zmienne do przechowywania danych
        int gold, diamond, chicken, ball, water;

        // Odczytaj dane z GlobalData
        GlobalData.Instance.ReadData(out gold, out diamond, out chicken, out ball, out water);

        // Ustaw wartość diamentu w TextMeshPro
        coinText.text = gold.ToString();
    }
}
