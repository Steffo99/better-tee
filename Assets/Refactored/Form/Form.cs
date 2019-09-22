using System.Collections.Generic;
using UnityEngine;

namespace BetterTee.Form {
    public class Form : MonoBehaviour {
        private Dictionary<string, FormItem> registeredItems;

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
    }
}