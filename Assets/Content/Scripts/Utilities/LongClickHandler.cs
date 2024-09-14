using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LongClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Public Variables
    public float holdTime = 2.0f;
    public Image fillImage;
    public UnityEvent onLongClickEvent;
    #endregion

    #region Private Variables
    private bool pointerDown;
    private float pointerDownTimer;
    #endregion

    #region Public Functions
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }
    #endregion

    #region Private Functions

    private void Update()
    {
        if(pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            if(pointerDownTimer > holdTime)
            {
                if(onLongClickEvent != null)
                    onLongClickEvent.Invoke();
                Reset();
            }
            fillImage.fillAmount = pointerDownTimer / holdTime;
        }
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        fillImage.fillAmount = pointerDownTimer / holdTime;
    }
    #endregion
}
