using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class InternetAndSSLCheck : MonoBehaviour
{
    public GameObject noInternetObject;   // Obiekt wyświetlany, gdy brak internetu
    public GameObject sslErrorObject;     // Obiekt wyświetlany, gdy problem z certyfikatem SSL
    public float checkInterval = 5f;      // Czas w sekundach pomiędzy kolejnymi sprawdzeniami
    private string url = "https://psiaapka.pl"; // URL do sprawdzenia

    void Start()
    {
        StartCoroutine(CheckInternetAndSSLRoutine());
    }

    IEnumerator CheckInternetAndSSLRoutine()
    {
        while (true)
        {
            bool isConnected = IsInternetAvailable();
            bool isSSLValid = false;

            if (isConnected)
            {
                // Jeśli jest internet, sprawdź SSL
                isSSLValid = IsSSLValid();
            }

            // Zarządzanie widocznością obiektów w zależności od wyników sprawdzeń
            noInternetObject.SetActive(!isConnected);
            sslErrorObject.SetActive(isConnected && !isSSLValid);

            yield return new WaitForSeconds(checkInterval);
        }
    }

    bool IsInternetAvailable()
    {
        try
        {
            // Sprawdza dostępność internetu przez wysyłanie żądania HTTP
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://google.com");
            request.Timeout = 5000; // Timeout na 5 sekund
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
        catch
        {
            // Brak internetu lub błąd sieci
            return false;
        }
    }

    bool IsSSLValid()
    {
        try
        {
            // Tworzymy żądanie HTTPS do serwera
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 5000; // Timeout na 5 sekund

            // Obsługa sprawdzenia certyfikatu SSL
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

            // Wysyłamy zapytanie i odbieramy odpowiedź
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                return true;
            }
        }
        catch
        {
            // Jeśli złapaliśmy wyjątek, oznacza to problem z SSL
            return false;
        }
    }

    // Funkcja sprawdzająca poprawność certyfikatu SSL
    public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            // Certyfikat jest poprawny
            return true;
        }
        else
        {
            // Certyfikat jest niepoprawny
            Debug.LogWarning("Błąd SSL: " + sslPolicyErrors);
            return false;
        }
    }
}
