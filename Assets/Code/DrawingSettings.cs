using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class DrawingSettings : ActSettings {
    public Color startingColor = Color.white;
    public List<Color> palette = new List<Color>();
    public float timeLimit = 99f;
    public string actName = "Untitled";
    public string actDescription = "This Act is missing a description.";
    public string destinationPool = "default";

    public DrawingSettings(Color startingColor, List<Color> palette, float timeLimit, string actName, string actDescription, string destinationPool) {
        this.type = "Drawing";
        this.startingColor = startingColor;
        this.palette = palette;
        this.timeLimit = timeLimit;
        this.actName = actName;
        this.actDescription = actDescription;
        this.destinationPool = destinationPool;
    }
}