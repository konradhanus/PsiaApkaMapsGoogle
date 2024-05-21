using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickBackButton : MonoBehaviour
{
    public void BackToGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
