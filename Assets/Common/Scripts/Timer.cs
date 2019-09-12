using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float startingTime = 0f;
    public float time = 0f;

    private bool isTriggered = false;
    private bool isRunning = false;    

    protected void Update() {
        if(time >= 0f) {
            if(isRunning) {
                time -= Time.deltaTime;
            }
        }
        else {
            if(isTriggered) {
                OnTimeOut();
                time = 0f;
                isTriggered = false;
                isRunning = false;
            }
        }
    }

    public void TimerSet(float startingTime) {
        isTriggered = true;
        isRunning = false;
        this.startingTime = startingTime;
        time = startingTime;        
    }

    public void TimerStart() {
        isRunning = true;
    }

    public void TimerPause() {
        isRunning = false;
    }

    public void TimerCancel() {
        time = 0f;
        isTriggered = false;
        isRunning = false;
    }

    public event Action OnTimeOut;
}
