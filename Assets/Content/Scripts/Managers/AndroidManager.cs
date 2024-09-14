using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using PedometerU;

public class AndroidManager : Singleton<AndroidManager>
{
    #region Public Variables
    #region Delegate
    public delegate void PopulateSteps(int steps, double distance);
    public event PopulateSteps OnPopulateSteps;
    #endregion
    #endregion

    #region Private Variables
    private Pedometer pedometer;
    private bool reading = false;
    #endregion

    #region Public Functions
    public void ReadPedometer()
    {
        if (!reading)
        {
            pedometer = new Pedometer(OnStep);
            OnStep(0, 0.00);
            reading = true;
        }
        else
        {
            StopPedometer();
            reading = false;
        }
    }
    #endregion

    #region Private Fuctions
    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission("android.permission.ACTIVITY_RECOGNITION"))
            {
                Permission.RequestUserPermission("android.permission.ACTIVITY_RECOGNITION");
            }
        }
        //if (Application.platform != RuntimePlatform.WindowsEditor)
        //{
        //    InputSystem.EnableDevice(StepCounter.current);
        //}

    }
    //private void Update()
    //{
    //    try
    //    {
    //        if (StepCounter.current.enabled)
    //        {
    //            StepCounter.current.samplingFrequency = 1;
    //            var stepValue = StepCounter.current.stepCounter.ReadValue();
    //            //stepsValue += stepValue;
    //            //stepText.text = stepValue.ToString();
    //        }
    //        else
    //        {
    //            //stepText.text = "not enabled";
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        //stepText.text = $"error: {e.Message}";
    //    }
    //}
    private void OnStep(int steps, double distance)
    {
        OnPopulateSteps(steps, distance);
        //UIManager.Instance.pedometerText.text = steps.ToString();
        //UIManager.Instance.pedometerTextButtonText.text = "Stop Pedometer";
        // Display the values // Distance in feet
        //stepText.text = steps.ToString();
        //distanceText.text = (distance * 3.28084).ToString("F2") + " ft";
    }

    private void StopPedometer()
    {
        // Release the pedometer
        pedometer?.Dispose();
        pedometer = null;
        //UIManager.Instance.pedometerTextButtonText.text = "Start Pedometer";
    }

    private void OnDisable()
    {
        StopPedometer();
    }
    #endregion
}
