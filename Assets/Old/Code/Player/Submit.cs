﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BetterTee.Player 
{
        
    public class Submit : MonoBehaviour
    {
        public InputField inputField;
        public TypingController typingController;

        protected EventSystem eventSystem;

        protected void Start() {
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
        }

        public void OnClick() {
            typingController.Submit(inputField.text);
            inputField.text = "";
            eventSystem.SetSelectedGameObject(inputField.gameObject);
        }
    }

}