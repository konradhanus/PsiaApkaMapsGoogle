using UnityEngine;
using System.Collections;

public class ShowDog : MonoBehaviour
{
    public GameObject[] dogs; // Tablica przechowująca wszystkie obiekty dog
    private int _dogType = 0;  // Iterator określający, który dog ma być pokazany

    private Animator animator; // Zmienna do przechowywania referencji do animatora
    private int[] possibleValues = new int[] { 4, 6, 7, 10, 12 }; // Możliwe losowe wartości

    private int lastRandomValue = -1; // Ostatnio wylosowana wartość

    public int DogType
    {
        get { return _dogType; }
        set
        {
            if (value < 0)
            {
                _dogType = dogs.Length - 1; // Jeśli wartość jest mniejsza od zera, ustawiamy dogType na ostatni element tablicy
            }
            else if (value >= dogs.Length)
            {
                _dogType = 0; // Jeśli wartość przekracza ilość elementów w tablicy, ustawiamy dogType na pierwszy element tablicy
            }
            else
            {
                _dogType = value; // W przeciwnym razie ustawiamy dogType na wartość dostarczoną przez użytkownika
            }
            
            UpdateDogVisibility();
        }
    }

    void UpdateDogVisibility()
    {
        // Iterowanie przez wszystkie obiekty dog w tablicy
        for (int i = 0; i < dogs.Length; i++)
        {
            // Jeśli indeks obiektu dog jest równy dogType, pokazujemy go, w przeciwnym razie ukrywamy
            if (i == _dogType)
            {
                dogs[i].SetActive(true); // Pokazanie obiektu dog
            }
            else
            {
                dogs[i].SetActive(false); // Ukrycie pozostałych obiektów dog
            }
        }
    }

    // Metoda do zmiany dogType o 1 w górę
    public void NextDog()
    {
        DogType++;
    }

    // Metoda do zmiany dogType o 1 w dół
    public void PreviousDog()
    {
        DogType--;
    }

    void Start()
    {
        // Sprawdź, czy obiekt ma przypisanego animatora
        if (GetComponent<Animator>() != null)
        {
            animator = GetComponent<Animator>(); // Przypisz animator do zmiennej
            StartCoroutine(RandomizeValueRoutine());
            // Ustaw parametr "Sleep_b" na true
          //  animator.SetBool("Sleep_b", true);
        }
        else
        {
            Debug.LogError("Obiekt nie ma przypisanego animatora.");
        }
    }

    int GetRandomValue()
    {
        int randomValue;
        do
        {
            randomValue = Random.Range(0, 5) switch
            {
                0 => 4,
                1 => 6,
                2 => 7,
                3 => 10,
                4 => 12,
                _ => 0,
            };
        } while (randomValue == lastRandomValue); // Sprawdzenie, czy wylosowana wartość jest taka sama jak poprzednio wylosowana

        lastRandomValue = randomValue; // Aktualizacja ostatnio wylosowanej wartości
        return randomValue;
    }
    
    IEnumerator RandomizeValueRoutine()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, possibleValues.Length); // Losowanie indeksu z tablicy possibleValues
            int randomValue = possibleValues[randomIndex]; // Pobranie losowej wartości z tablicy
            animator.SetInteger("ActionType_int", randomValue);  // Ustawienie losowej wartości

            yield return new WaitForSeconds(4f); // Oczekiwanie przez 2 sekundy

            animator.SetInteger("ActionType_int", 0); // Ustawienie wartości na 0 po 2 sekundach

            yield return new WaitForSeconds(10f); // Oczekiwanie przez kolejne 5 sekund przed ponownym losowaniem
        }
    }
}
