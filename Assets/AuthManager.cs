using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class authManager : MonoBehaviour
{
    public Text logTxt;
    [HideInInspector]public string facebookToken = "....";
    async void Start() {
        await UnityServices.InitializeAsync();
        // SignIn();
    }

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public async void SignIn() {
        await signInAnonymous();
    }

    async Task signInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            print("Sign in Success");
            PlayGame();
            // print("Player Id:" + AuthenticationService.Instance.PlayerId);
           // logTxt.text = "Player id:" + AuthenticationService.Instance.PlayerId;
        }
        catch (AuthenticationException ex)
        {
            print("Sign in failed!!");
            Debug.LogException(ex);
        }
    
    }

    async Task signInWithFacebook(string token) {

        try
        {
            await AuthenticationService.Instance.SignInWithFacebookAsync(token);

            print("Sign in with facebook success");
        }
        catch (AuthenticationException ex)
        {
            print("Sign in failed!!");
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
    async Task signInWithSteam(string token)
    {

        try
        {
            await AuthenticationService.Instance.SignInWithSteamAsync(token);

            print("Sign in with facebook success");
        }
        catch (AuthenticationException ex)
        {
            print("Sign in failed!!");
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

}