using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

using Firebase;
using Firebase.Auth;

public class AuthManagerGame : MonoBehaviour
{
    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    public TMP_Text nickName;

    private string loggedAs; 
    private string previousLoggedAs;

    private void Awake()
    {
        // Check that all of the necessary dependencies for firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Map Signed out " + user.UserId);
            }

            user = auth.CurrentUser;
            if (user != null) {
                Debug.Log("pobrano dane uzytkownika");
                string name = user.DisplayName;
                string email = user.Email;
                System.Uri photo_url = user.PhotoUrl;
                // The user's Id, unique to the Firebase project.
                // Do NOT use this value to authenticate with your backend server, if you
                // have one; use User.TokenAsync() instead.
                string uid = user.UserId;
                loggedAs = email;
                
            }
            if (signedIn)
            {
                Debug.Log("Map Signed in " + user.UserId);
                ReferencesUserFirebase.userId = user.UserId;
                // ReferencesUserFirebase.userName = user.DisplayName;
                string jsonString = JsonUtility.ToJson(auth);
                Debug.Log(jsonString);
                Debug.Log(auth);
            }
        }
    }

    void Start()
    {
        UpdateDisplayName();
    }

    void Update()
    {
        // Aktualizacja wyświetlanego nicku tylko wtedy, gdy wartość loggedAs się zmieniła
        if (loggedAs != previousLoggedAs)
        {
            UpdateDisplayName();
        }
    }

    void UpdateDisplayName()
    {
        previousLoggedAs = loggedAs;
        if (loggedAs != null)
        {
            string displayName = GetUsernameFromEmail(loggedAs);
            nickName.text = displayName;
        }
    }

    public void LogOut()
    {
        auth.SignOut();
        SceneManager.LoadSceneAsync(0);
    }


    public string GetUsernameFromEmail(string email)
    {
        int atIndex = email.IndexOf('@');
        if (atIndex != -1)
        {
            return email.Substring(0, atIndex);
        }
        else
        {
            // Jeśli nie ma znaku '@' w adresie e-mail, zwróć cały adres
            return email;
        }
    }
}
