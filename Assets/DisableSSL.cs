using UnityEngine;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class GlobalSSLHandler : MonoBehaviour
{
    // Ustaw ten obiekt jako globalny, żeby przetrwał zmianę scen
    void Awake()
    {
        // Wyłącz weryfikację SSL globalnie
        ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
        DontDestroyOnLoad(this.gameObject);
    }

    private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // Zawsze akceptuj certyfikat
        return true;
    }
}
