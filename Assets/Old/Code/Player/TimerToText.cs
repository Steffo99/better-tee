using UnityEngine;
using UnityEngine.UI;

namespace BetterTee.Player 
{

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

}
