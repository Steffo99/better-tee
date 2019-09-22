using UnityEngine;
using UnityEngine.UI;

namespace BetterTee.Player {

    public class RadiusSlider : MonoBehaviour
    {
        public PencilTool pencil;

        protected Slider slider;

        protected void Start() {
            slider = GetComponent<Slider>();
        }

        public void OnValueChange()
        {
            pencil.size = slider.value;
        }
    }

}
