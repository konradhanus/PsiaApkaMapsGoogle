using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopwatchManager : Singleton<StopwatchManager>
{
    #region Public Variables
    public TimeSpan currentTimeSpan;
    //[HideInInspector]
    public float currentTime;
    //[HideInInspector]
    public float currentScoringTime;

    #region Delegate
    //public delegate void MinuteThreshold();
    //public event MinuteThreshold onAchievedMinuteThreshold;
    #endregion

    #endregion

    #region Private Variables
    private bool stopWatchEnabled = false;
    //private float minuteThresholdHandler;
    //private int minutePassed = 1;
    #endregion

    #region Public Functions
    public void StartStopWatch()
    {
        stopWatchEnabled = true;
    }

    public void StopStopWatch()
    {
        stopWatchEnabled = false;
    }

    public void ResetStopWatch()
    {
        //minutePassed = 1;
        currentTime = 0f;
        currentScoringTime = 0f;
        currentTimeSpan = TimeSpan.Zero;
    }
    #endregion

    #region Private Functions
    private void Update()
    {
        if(stopWatchEnabled)
        {
            if(GameplayManager.Instance.continueEarningSteps)
            {
                currentScoringTime += Time.deltaTime;
            }
            currentTime += Time.deltaTime;
            currentTimeSpan = TimeSpan.FromSeconds(currentTime);
            //minuteThresholdHandler = currentTime;
            //if (minuteThresholdHandler > (60 * minutePassed) + 1)
            //{
            //    minutePassed++;
            //    onAchievedMinuteThreshold();
            //    minuteThresholdHandler = 0;
            //}

        }

    }

    //private void setTime
    #endregion
}
