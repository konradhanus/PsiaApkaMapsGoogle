using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Auth;

using TMPro;
using UnityEngine.Networking;

public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public GameObject blackScreen;

    // Login Variables
    [Space]
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    public GameObject SuccessScreen;
    public GameObject LoginScreen;
    public GameObject FailureScreen;

    public Text logger;

    bool loogedIn = false;

    public GameObject avatar;
    public GameObject dogAvatar;
    private int selectedDogAvatar;
    private int selectedGender;

    private void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    void Start()
    {
        if (blackScreen != null)
        {
            // Uruchom Coroutine, aby wyłączyć blackScreen po 2 sekundach
            StartCoroutine(DisableBlackScreenAfterDelay(2.0f));
        }
    }

    private IEnumerator DisableBlackScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Czekaj przez określony czas
        blackScreen.SetActive(false);           // Wyłącz blackScreen
    }
    
    void Update()
    {
        if(loogedIn)
        {
            PlayGame();
            // black screen on
        }else{
            
            //black screen off
        }
    }

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
                logger.text = "Could not resolve all firebase dependencies: " + dependencyStatus;
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
                Debug.Log("Signed out " + user.UserId);
                loogedIn = false;
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("AuthStateChanged Signed in " + user.UserId);
                Debug.Log("USER2:" + user.UserId);
                loogedIn = true;

            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(emailLoginField.text, passwordLoginField.text));
    }

    public IEnumerator LoginAsync(string email, string password)
    {
        Debug.Log("LoginAsync");
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "Login Failed! Because ";
           

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "Email is invalid";
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "Wrong Password";
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "Email is missing";
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "Password is missing";
                    break;
                default:
                    failedMessage = "Login Failed";
                    break;
            }
            blackScreen.SetActive(false);
            // logger.text = failedMessage;

            Debug.Log(failedMessage);
        }
        else
        {
            user = loginTask.Result.User;

            ReferencesUserFirebase.userId = user.UserId;
            ReferencesUserFirebase.userName = user.DisplayName;
            
            Debug.LogFormat("{0} You Are Successfully Logged In", user.DisplayName);
            

            // logger.text = user.DisplayName;
            if (!string.IsNullOrEmpty(user.UserId))
            {
                 PlayGame();
                
            }else{
                  Debug.LogError("jest pusty");
                //    logger.text = "jest pusty";
            }
        }
    }

    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }

    // Coroutine do wysyłania danych do endpointu
    private IEnumerator SendRegistrationData(string UUID, string nick, int selectedGender, int dogAvatar)
    {
        string url = $"https://psiaapka.pl/psiaapka/create_user_account.php?id_avatar={selectedGender}&id_avatar_dog={dogAvatar}&nick={nick}&UUID={UUID}";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("USER Error while sending data: " + webRequest.error);
            }
            else
            {
                Debug.Log("USER Data successfully sent: " + webRequest.downloadHandler.text);
            }
        }
    }

    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("User Name is empty");
            logger.text = "User Name is empty";
        }
        else if (email == "")
        {
            Debug.LogError("email field is empty");
            logger.text = "email field is empty";
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            Debug.LogError("Password does not match");
            logger.text = "Password does not match";
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration Failed! Becuase ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "Email is invalid";
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "Wrong Password";
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "Email is missing";
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "Password is missing";
                        break;
                    default:
                        failedMessage = "Registration Failed";
                        break;
                }
                logger.text = failedMessage;
                Debug.Log(failedMessage);
            }
            else
            {
                // Get The User After Registration Success
                user = registerTask.Result.User;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
                Debug.Log("USER JUST AFTER REGISTER" + user.UserId);

                if (avatar != null)
                {
                    AvatarSetter avatarSetter = avatar.GetComponent<AvatarSetter>();
                    if (avatarSetter != null)
                    {
                        selectedGender = avatarSetter.selectedGender;
                        Debug.Log("Selected Gender: " + selectedGender);
                    }
                    else
                    {
                        Debug.LogError("Brak komponentu AvatarSetter w targetGameObject.");
                    }
                }
                else
                {
                    Debug.LogError("targetGameObject jest null.");
                }

                if (dogAvatar != null)
                {
                    ShowDog avatarDogSetter = dogAvatar.GetComponent<ShowDog>();
                    if (avatarDogSetter != null)
                    {
                        selectedDogAvatar = avatarDogSetter._dogType;
                        Debug.Log("Selected Dog Avatar: " + selectedDogAvatar);
                    }
                    else
                    {
                        Debug.LogError("Brak komponentu ShowDog w targetGameObject.");
                    }
                }
                else
                {
                    Debug.LogError("targetGameObject jest null.");
                }

                StartCoroutine(SendRegistrationData(user.UserId, name, selectedGender, selectedDogAvatar));

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;


                    string failedMessage = "Profile update Failed! Becuase ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "Email is invalid";
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "Wrong Password";
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "Email is missing";
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "Password is missing";
                            break;
                        default:
                            failedMessage = "Profile update Failed";
                            break;
                    }
                    logger.text = failedMessage;
                    Debug.Log(failedMessage);
                }
                else
                {
                    Debug.Log("Registration Sucessful Welcome " + user.DisplayName);
                    logger.text = "Registration Sucessful Welcome " + user.DisplayName;

                    Debug.Log("USER" + user);
                    SuccessScreen.SetActive(true);
                    LoginScreen.SetActive(false);
                    // UIManager.Instance.OpenLoginPanel();
                }
            }
        }
    }
}
