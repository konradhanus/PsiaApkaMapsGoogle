using UnityEngine;
using TMPro;

public class GetBallValue : MonoBehaviour
{
    public TextMeshProUGUI ballText;

    void OnEnable()
    {
        // Deklaruj zmienne do przechowywania danych
        int gold, diamond, chicken, ball, water;

        // Odczytaj dane z GlobalData
        GlobalData.Instance.ReadData(out gold, out diamond, out chicken, out ball, out water);

        // Ustaw wartość diamentu w TextMeshPro
        ballText.text = ball.ToString();
    }
}
