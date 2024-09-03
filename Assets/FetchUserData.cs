using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;  // Dodaj to, aby mieć dostęp do komponentu Image

public class GlobalUserData : MonoBehaviour
{
    public bool debugMode = true;
    public static GlobalUserData Instance;

    public string defaultAccount = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    public string userId = "eOexsqawm4YO9GhnmYT9Ka7RbRq1";
    private FirebaseAuthManager authManager;

    public GameObject PlayerArmature;

    public Avatar WomanAvatar;
    public Avatar ManAvatar;
    public GameObject Woman;
    public GameObject Man;

    public GameObject WomanProfile;
    public GameObject ManProfile;

    public Sprite WomanCircleAvatarImage;
    public Sprite ManCircleAvatarImage;

    public GameObject PlayerAvatar;
    public GameObject DogAvatar;

    public Sprite[] DogsCircleAvatar; // Tablica dla wszystkich psów

    public GameObject[] Dogs; // Tablica dla wszystkich psów
    public GameObject[] DogsProfile; // Tablica dla wszystkich psów
    public GameObject[] DogsCoupleProfile; // Tablica dla wszystkich psów

    UserData userData;

    public TextMeshProUGUI nickNameText;

    [System.Serializable]
    public class DataResponse
    {
        public UserData data;
    }

    [System.Serializable]
    public class UserData
    {
        public string nick;
        public int id_avatar;
        public int id_avatar_dog;
        public string date_created;
    }

    void Start()
    {
        if (debugMode)
        {
            // Debug mode logic
        }
        else
        {
            authManager = new FirebaseAuthManager();
            userId = ReferencesUserFirebase.userId;

            print("Player Id:" + userId);

            if (userId == "")
            {
                Debug.Log("FetchUserData: pusty");
                userId = defaultAccount;
            }

        }
        Debug.Log("FetchUserData: FETCH! JESTEM PONOWNIE!: "+ userId);
        StartCoroutine(UpdateDataFromAPI());
    }

    public void FetchData()
    {
        Debug.Log("FetchUserData: FETCH!, fetch data");
        Debug.Log("FetchUserData: userId" + userId);
        StartCoroutine(UpdateDataFromAPI());
    }

    IEnumerator UpdateDataFromAPI()
    {
        string url = "https://psiaapka.pl/psiaapka/userData.php?user_id=" + userId;

        Debug.Log("FetchUserData: FETCH! " + url);
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("FetchUserData: Błąd podczas pobierania danych z API: " + request.error);
            yield break;
        }

        string jsonResponse = request.downloadHandler.text;
        DataResponse dataResponse = JsonUtility.FromJson<DataResponse>(jsonResponse);

        if (dataResponse != null && dataResponse.data != null)
        {
            Debug.Log("FetchUserData: FETCH!!!" + dataResponse);
            userData = dataResponse.data;
            UpdateUserData(dataResponse.data);
        }
        else
        {
            Debug.LogError("FetchUserData: Błąd podczas parsowania odpowiedzi z API.");
        }
    }

    public void UpdateUserData(UserData userData)
    {
        Debug.Log("FetchUserData: FETCH! Nick: " + userData.nick + ", SkinId: " + userData.id_avatar + ", DogSkinId: " + userData.id_avatar_dog + ", Registration Date: " + userData.date_created);

        // Update UI elements with received data
        // nickNameText.text = userData.nick;

        // Update avatar and character visibility based on skinId
        Debug.Log("FetchUserData: FETCH!" + userData.id_avatar);

        if (PlayerArmature == null)
        {
            Debug.LogError("FetchUserData: PlayerArmature is not assigned.");
            return;
        }

        Animator playerAnimator = PlayerArmature.GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("FetchUserData: Animator component not found on PlayerArmature.");
            return;
        }

        // Get the Image component from PlayerAvatar
        Image avatarImage = PlayerAvatar.GetComponent<Image>();
        if (avatarImage == null)
        {
            Debug.LogError("FetchUserData: Image component not found on PlayerAvatar.");
            return;
        }

        Debug.Log("FetchUserData: FETCH!" + (userData.id_avatar == 0));
        if (userData.id_avatar == 0)
        {
            Debug.Log("FetchUserData: FETCH! WOMAN");
            playerAnimator.avatar = WomanAvatar;
            Woman.SetActive(true);
            Man.SetActive(false);

            WomanProfile.SetActive(true);
            ManProfile.SetActive(false);

            avatarImage.sprite = WomanCircleAvatarImage;  // Zmieniamy sprite
        }
        else if (userData.id_avatar == 1)
        {
            Debug.Log("FetchUserData: FETCH! MAN");
            playerAnimator.avatar = ManAvatar;
            Woman.SetActive(false);
            Man.SetActive(true);

            WomanProfile.SetActive(false);
            ManProfile.SetActive(true);

            avatarImage.sprite = ManCircleAvatarImage;  // Zmieniamy sprite 
        }
        Debug.Log("FetchUserData: FETCH!" + userData.id_avatar_dog);
        // Update the visibility of the dogs based on the id_avatar_dog
        ShowDog(userData.id_avatar_dog);
    }

    private void ShowDog(int dogId)
    {
        Debug.Log("FetchUserData: FETCH!XXX" + dogId);
        // Ukryj wszystkie psy
        foreach (GameObject dog in Dogs)
        {
            dog.SetActive(false);
        }

        foreach (GameObject dogprofile in DogsProfile)
        {
            dogprofile.SetActive(false);
        }

        foreach (GameObject dogcoupleprofile in DogsCoupleProfile)
        {
            dogcoupleprofile.SetActive(false);
        }

        int index;
        if (dogId >= 0 && dogId <= Dogs.Length)
        {
            index = dogId; // Przekształcenie dogId (1-28) na indeks (0-27)
        }
        else
        {
            index = 24; // Domyślnie ustaw psa nr 14 (indeks 13)
        }

        if (DogAvatar == null)
        {
            Debug.LogError("FetchUserData: DogAvatar is not assigned.");
            return;
        }

        // Get the Image component from DogAvatar
        Image avatarDogImage = DogAvatar.GetComponent<Image>();
        if (avatarDogImage == null)
        {
            Debug.LogError("FetchUserData: Image component not found on DogAvatar.");
            return;
        }

        avatarDogImage.sprite = DogsCircleAvatar[index];
        Dogs[index].SetActive(true);
        DogsProfile[index].SetActive(true);
        DogsCoupleProfile[index].SetActive(true);
    }

    public void ReadData(out string nick, out int skinId, out int dogSkinId, out string date_created)
    {
        // Return data from API response
        nick = nickNameText.text;
        // Directly assign the integer values from userData
        skinId = userData.id_avatar;
        dogSkinId = userData.id_avatar_dog;
        date_created = userData.date_created;
    }
}
