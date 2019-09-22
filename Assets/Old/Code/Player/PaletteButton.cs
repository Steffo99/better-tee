using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BetterTee.Player {

    public class PaletteButton : MonoBehaviour
    {
        public PencilTool pencil;

        protected Image image;

        protected void Start() 
        {
            image = GetComponent<Image>();
        }

        public void OnClick()
        {
            pencil.selectedColor = image.color;
        }
    }

}
