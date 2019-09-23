using System;
using UnityEngine;

namespace BetterTee.Form {
    public abstract class FormEvent : FormElement {
        public event Action<FormEvent> OnTrigger;

        protected void TriggerEvent() {
            OnTrigger(this);
        }

        public void UnsubscribeAll() {
            OnTrigger = null;
        }
    }
}
