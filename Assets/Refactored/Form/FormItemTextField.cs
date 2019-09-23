using UnityEngine;
using UnityEngine.UI;

namespace BetterTee.Form {
    [RequireComponent(typeof(InputField))]
    public class FormItemTextField : FormItem {
        protected InputField inputFieldComponent;

        protected void Start() {
            inputFieldComponent = GetComponent<inputFieldComponent>();
        }

        public override dynamic Value {
            get {
                return inputFieldComponent.text;
            }

            set {
                inputFieldComponent.text = value;
            }
        }
    }
}
