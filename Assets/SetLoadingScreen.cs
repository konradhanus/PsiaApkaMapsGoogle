using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLoadingScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickButton(){
        LoadingScreenStore.isVisible = true;
    }
}
