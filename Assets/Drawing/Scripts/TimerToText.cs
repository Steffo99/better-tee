using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerToText : MonoBehaviour
{
    protected Timer timer;
    protected Text text;

    protected void Start() 
    {
        timer = GetComponent<Timer>();
        text = GetComponent<Text>();
    }

    public void Update()
    {
        text.text = Mathf.CeilToInt(timer.time).ToString();
    }
}
