using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LayerLab
{
    public class PanelSimpleCasual : MonoBehaviour
    {
        [SerializeField] private GameObject[] otherPanels;


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
        public void ShowCoin(int value)
        {
            ResetIcons();
            coinIcon.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
        }

        // Metoda do ustawienia widoczności tylko ikony i wartości diamentu
        public void ShowDiamond(int value)
        {
            ResetIcons();
            diamondIcon.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
        }

        // Metoda do ustawienia widoczności tylko ikony i wartości skrzyni
        public void ShowChest(int value)
        {
            ResetIcons();
            chestIcon.SetActive(true);
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
        }

        // Metoda do ustawienia widoczności tylko ikony i wartości jedzenia
        public void ShowFood(int value)
        {
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
        public void ShowBag(int value)
        {
            ResetIcons();
            bagIcon.SetActive(true); // Jeśli backIcon oznacza torbę, można to zmienić
            title.GetComponent<TextMeshProUGUI>().text = "Nagroda";
            textValue.text = value.ToString();
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
