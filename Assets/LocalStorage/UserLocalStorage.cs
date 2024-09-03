using UnityEngine;

// Klasa do zarządzania danymi użytkownika w lokalnej pamięci
public static class UserLocalStorage
{
    // Klucz do przechowywania ID użytkownika w PlayerPrefs
    private const string UserIdKey = "UserId";
    private const string UserDataKey = "UserData";

    // Metoda do zapisywania ID użytkownika
    public static void SaveUserId(string userId)
    {
        PlayerPrefs.SetString(UserIdKey, userId);
        PlayerPrefs.Save();
        Debug.Log("UserLocalStorage: User ID saved.");
    }

    // Metoda do odczytywania ID użytkownika
    public static string LoadUserId()
    {
        if (PlayerPrefs.HasKey(UserIdKey))
        {
            string userId = PlayerPrefs.GetString(UserIdKey);
            Debug.Log("UserLocalStorage: User ID loaded: " + userId);
            return userId;
        }

        Debug.LogWarning("UserLocalStorage: No User ID found.");
        return null;
    }

    // Metoda do usuwania ID użytkownika
    public static void DeleteUserId()
    {
        if (PlayerPrefs.HasKey(UserIdKey))
        {
            PlayerPrefs.DeleteKey(UserIdKey);
            Debug.Log("UserLocalStorage: User ID deleted.");
        }
    }

    // Metoda do zapisywania danych użytkownika
    public static void SaveUserData(UserData userData)
    {
        string jsonData = JsonUtility.ToJson(userData);
        PlayerPrefs.SetString(UserDataKey, jsonData);
        PlayerPrefs.Save();
        Debug.Log("UserLocalStorage: User data saved.");
    }

    // Metoda do odczytywania danych użytkownika
    public static UserData LoadUserData()
    {
        if (PlayerPrefs.HasKey(UserDataKey))
        {
            string jsonData = PlayerPrefs.GetString(UserDataKey);
            UserData userData = JsonUtility.FromJson<UserData>(jsonData);
            Debug.Log("UserLocalStorage: User data loaded.");
            return userData;
        }

        Debug.LogWarning("UserLocalStorage: No user data found.");
        return null;
    }

    // Metoda do usuwania danych użytkownika
    public static void DeleteUserData()
    {
        if (PlayerPrefs.HasKey(UserDataKey))
        {
            PlayerPrefs.DeleteKey(UserDataKey);
            Debug.Log("UserLocalStorage: User data deleted.");
        }
    }
}

// Klasa reprezentująca dane użytkownika
[System.Serializable]
public class UserData
{
    public string nickname;
    public int avatarId;
    public int dogAvatarId;
    public string registrationDate;

    public UserData(string nickname, int avatarId, int dogAvatarId, string registrationDate)
    {
        this.nickname = nickname;
        this.avatarId = avatarId;
        this.dogAvatarId = dogAvatarId;
        this.registrationDate = registrationDate;
    }
}
