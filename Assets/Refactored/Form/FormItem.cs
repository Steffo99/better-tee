using UnityEngine;

namespace BetterTee.Form {
    public abstract class FormItem : MonoBehaviour {
        public string Name;

        public abstract dynamic Value {get; set;}
    }
}
