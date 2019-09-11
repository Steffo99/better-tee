using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float startingTime = 0f;
    public float time = 0f;
    public bool isTriggered = false;
    public bool isRunning = false;    

    protected void Update() {
        if(time >= 0f) {
            if(isRunning) {
                time -= Time.deltaTime;
            }
        }
        else {
            time = 0f;
            if(isTriggered) {
                OnTimeOut(EventArgs.Empty);
                isTriggered = false;
            }
        }
    }

    public void StartTimer(float startingTime) {
        isTriggered = true;
        isRunning = true;
        this.startingTime = startingTime;
        time = startingTime;        
    }

    public event EventHandler TimeOut;
    protected virtual void OnTimeOut(EventArgs e)
    {
        EventHandler handler = TimeOut;
        handler?.Invoke(this, e);
    }
}
