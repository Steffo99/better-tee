using UnityEngine.UI;

namespace BetterTee.Form {
    public class FormItemTextField : FormItem {
        protected InputField inputFieldComponent;

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
