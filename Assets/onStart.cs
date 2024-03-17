using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onStart : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject element;

    void Start()
    {
        if (element != null)
        {
            element.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
