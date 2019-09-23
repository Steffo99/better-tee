using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterTee.Form {
    public class Form : MonoBehaviour {
        private Dictionary<string, FormItem> registeredItems;
        private Dictionary<string, FormEvent> registeredButtons;

        protected void Start() {
            registeredItems = new Dictionary<string, FormItem>();
        }

        public void RegisterItem(FormItem item) {
            registeredItems.Add(item.name, item);
        }

        public void UnregisterItem(string name) {
            registeredItems.Remove(name);
        }

        public FormItem GetItem(string name) {
            return registeredItems[name];
        }

        public dynamic GetValue(string name) {
            return registeredItems[name].Value;
        }

        public void SetValue(string name, dynamic value) {
            registeredItems[name].Value = value;
        }

        public void RegisterButton(FormEvent button) {
            registeredButtons.Add(button.name, button);
        }

        public void UnregisterButton(string name) {
            FormEvent button = registeredButtons[name];
            button.UnsubscribeAll();
            registeredButtons.Remove(name);
        }

        public void SubscribeTo(string name, Action<FormEvent> handler) {
            registeredButtons[name].OnTrigger += handler;
        }
    }
}