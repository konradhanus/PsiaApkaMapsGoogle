using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace LayerLab
{
    public class PanelSimpleCasual : MonoBehaviour
    {
        public GameObject Resources; // Obiekt, który zawiera GlobalData
        private string uuid; // UUID przypisywane z GlobalData

        public void getUUID()
        {
            // Pobranie UserId z GlobalData i ustawienie UUID
            GlobalData globalData = Resources.GetComponent<GlobalData>();
            if (globalData != null)
            {
                uuid = globalData.userId; // Przypisanie UserId do zmiennej uuid                    
            }
            
        }

        void Start()
        {
            getUUID();
        }

        [SerializeField] private GameObject[] otherPanels;
        private static readonly HttpClient httpClient = new HttpClient();

        // Dodane elementy UI
        [SerializeField] private GameObject title; // Tytuł

        [SerializeField] private GameObject coinIcon; // Ikona monety
        [SerializeField] private GameObject diamondIcon; // Ikona diamentu
        [SerializeField] private GameObject bagIcon; // Ikona powrotu
        [SerializeField] private GameObject chestIcon; // Ikona skrzyni
        [SerializeField] private GameObject foodIcon; // Ikona jedzenia

        //[SerializeField] private GameObject ballIcon; // Ikona piłki

        [SerializeField] private TextMeshProUGUI textValue; // Tekst wartości

        public void OnEnable()
        {
            for (int i = 0; i < otherPanels.Length; i++) otherPanels[i].SetActive(true);
        }

        public void OnDisable()
        {
            for (int i = 0; i < otherPanels.Length; i++) otherPanels[i].SetActive(false);
        }

        // Metoda do ustawienia widoczności tylko ikony i wartości monety
        public async void ShowCoin(int value)
        {
            getUUID();
            string url = $"https://psiaapka.pl/updateReward.php?reward_play=1&uuid={uuid}";
            // Wywołanie API
            await CallApi(url);

            ResetIcons();
            coinIcon.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
        }

        // Metoda do ustawienia widoczności tylko ikony i wartości diamentu
        public async void ShowDiamondAsync(int value)
        {
            getUUID();
            string url = $"https://psiaapka.pl/updateReward.php?reward_treat=1&uuid={uuid}";
            // Wywołanie API
            await CallApi(url);

            ResetIcons();
            diamondIcon.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
        }

        // Metoda do ustawienia widoczności tylko ikony i wartości skrzyni
        public async void ShowChestAsync(int value)
        {
            getUUID();
            // URL API, do którego zostanie wysłane zapytanie

            string url = $"https://psiaapka.pl/updateReward.php?reward_water=1&uuid={uuid}";
            // Wywołanie API
            await CallApi(url);

            ResetIcons();
            chestIcon.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();


        }

        // Metoda do ustawienia widoczności tylko ikony i wartości jedzenia
        public async void ShowFood(int value)
        {
            getUUID();
            string url = $"https://psiaapka.pl/updateReward.php?reward_walk=1&uuid={uuid}";
            // Wywołanie API
            await CallApi(url);

            ResetIcons();
            foodIcon.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
        }

        //// Metoda do ustawienia widoczności tylko ikony i wartości piłki
        //public void ShowBall(int value)
        //{
        //    ResetIcons();
        //    ballIcon.SetActive(true);
        //    title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
        //    textValue.text = value.ToString();
        //}

        // Metoda do ustawienia widoczności tylko ikony i wartości torby
        public async void ShowBag(int value)
        {
            getUUID();
            string url = $"https://psiaapka.pl/updateReward.php?reward_treasure=1&uuid={uuid}";
            // Wywołanie API
            await CallApi(url);

            ResetIcons();
            bagIcon.SetActive(true); // Jeśli backIcon oznacza torbę, można to zmienić
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
        }

        private async Task CallApi(string url)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Debug.Log("API response: " + responseBody);
                }
                else
                {
                    Debug.LogError("Błąd w wywołaniu API: " + response.StatusCode);
                }
            }
            catch (HttpRequestException e)
            {
                Debug.LogError("Wystąpił wyjątek w czasie wywołania API: " + e.Message);
            }
        }

        // Resetuje widoczność wszystkich ikon
        private void ResetIcons()
        {
            coinIcon.SetActive(false);
            diamondIcon.SetActive(false);
            bagIcon.SetActive(false);
            chestIcon.SetActive(false);
            foodIcon.SetActive(false);
            //ballIcon.SetActive(false);
        }
    }
}
