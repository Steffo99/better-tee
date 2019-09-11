using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
