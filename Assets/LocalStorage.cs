using UnityEngine;

public class UserManager : MonoBehaviour
{
    void Start()
    {
        // Przykładowe użycie
        SaveCurrentUser();
        LoadCurrentUser();
    }

    public void SaveCurrentUser()
    {
        // Przykład zapisu użytkownika
        UserLocalStorage.SaveUserId("unique_user_id");

        UserData userData = new UserData("Player1", 0, 1, "2024-09-02");
        UserLocalStorage.SaveUserData(userData);

        Debug.Log("UserManager: Current user saved.");
    }

    public void LoadCurrentUser()
    {
        // Przykład odczytu użytkownika
        string userId = UserLocalStorage.LoadUserId();
        UserData userData = UserLocalStorage.LoadUserData();

        if (userId != null && userData != null)
        {
            Debug.Log("UserManager: Loaded User ID: " + userId);
            Debug.Log("UserManager: Loaded User Data: " + userData.nickname);
        }
        else
        {
            Debug.LogWarning("UserManager: No user data found.");
        }
    }
}
