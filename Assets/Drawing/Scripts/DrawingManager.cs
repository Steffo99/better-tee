using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DrawingSettings {
    public Color startingColor = Color.white;
    public List<Color> palette = new List<Color>();
    public float timeLimit = 99f;
    public string actName = "Untitled";
    public string actDescription = "This Act is missing a description.";
}

public class DrawingManager : MonoBehaviour
{
    [Header("Settings")]
    public string jsonString = "";
    public DrawingSettings settings = null;

    [Header("Prefabs")]
    public GameObject drawableFramePrefab;
    public GameObject paletteButtonPrefab;
    public GameObject radiusSliderPrefab;
    public GameObject actNamePrefab;
    public GameObject actDescriptionPrefab;
    public GameObject timerPrefab;

    [Header("Objects")]
    protected Canvas canvas;
    protected PencilTool pencil;
    protected DrawableFrame drawableFrame;
    protected List<PaletteButton> paletteButtons;
    protected RadiusSlider radiusSlider;
    protected Text actName;
    protected Text actDescription;
    protected Timer timer;

    [Header("Results")]
    public byte[] png = null;

    void Start() {
        if(jsonString != "") {
            JsonUtility.FromJsonOverwrite(jsonString, settings);
            if(settings == null) {
                Debug.LogWarning("Invalid settings json string, using defaults.");
            }
        }
        else {
            Debug.Log(JsonUtility.ToJson(settings));
        }
        
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();

        drawableFrame = Instantiate(drawableFramePrefab, transform).GetComponent<DrawableFrame>();
        drawableFrame.startingColor = settings.startingColor;

        pencil = drawableFrame.GetComponent<PencilTool>();
        try {
            pencil.selectedColor = settings.palette[0];
        } catch(ArgumentOutOfRangeException) {
            pencil.selectedColor = settings.startingColor;
        }

        paletteButtons = new List<PaletteButton>();
        for(int i = 0; i < settings.palette.Count; i++) {
            PaletteButton button = Instantiate(paletteButtonPrefab, canvas.transform).GetComponent<PaletteButton>();
            RectTransform btnTransform = button.GetComponent<RectTransform>();
            Image btnImage = button.GetComponent<Image>();
            button.pencil = pencil;
            btnImage.color = settings.palette[i];
            btnTransform.anchoredPosition = new Vector2(-420 + i * 110, 150);
            paletteButtons.Add(button);
        }

        radiusSlider = Instantiate(radiusSliderPrefab, canvas.transform).GetComponent<RadiusSlider>();
        radiusSlider.pencil = pencil;

        actName = Instantiate(actNamePrefab, canvas.transform).GetComponent<Text>();
        actName.text = settings.actName;

        actDescription = Instantiate(actDescriptionPrefab, canvas.transform).GetComponent<Text>();
        actDescription.text = settings.actDescription;

        timer = Instantiate(timerPrefab, canvas.transform).GetComponent<Timer>();
        timer.TimerSet(settings.timeLimit);
        timer.OnTimeOut += ActEnd;

        ActStart();
    }

    void ActStart() {
        timer.TimerStart();
        drawableFrame.locked = false;
    }

    void ActEnd(object sender, EventArgs e) {
        drawableFrame.locked = true;
        png = drawableFrame.ToPNG();
    }
}
