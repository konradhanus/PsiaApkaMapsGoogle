using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Android;
using System;

public class UnityStepCounter : MonoBehaviour
{
    public Text stepText;
    private int stepsValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
        {
            Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
        }
        InputSystem.EnableDevice(StepCounter.current);

    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (StepCounter.current.enabled)
            {
                StepCounter.current.samplingFrequency = 1;
                var stepValue = StepCounter.current.stepCounter.ReadValue();
                //stepsValue += stepValue;
                stepText.text = stepValue.ToString();
            }
            else
            {
                stepText.text = "not enabled";
            }
        }
        catch(Exception e)
        {
            stepText.text = $"error: {e.Message}";
        }
    }
}
