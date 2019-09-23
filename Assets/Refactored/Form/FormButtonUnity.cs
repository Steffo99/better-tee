using UnityEngine;
using UnityEngine.UI;

namespace BetterTee.Form {
    [RequireComponent(typeof(Button))]
    public class FormButtonUnity : FormEvent {
        protected Button buttonComponent;

        private void Start() {
            buttonComponent = GetComponent<Button>();
            buttonComponent.onClick.AddListener(ButtonComponent_OnClick);
        }

        private void ButtonComponent_OnClick() {
            TriggerEvent();
        } 
    }
}
