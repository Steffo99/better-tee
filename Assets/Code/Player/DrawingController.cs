using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BetterTee.Player 
{
    
    public class DrawingController : ActController
    {
        [Header("Prefabs")]
        public GameObject drawableFramePrefab;
        public GameObject paletteButtonPrefab;
        public GameObject radiusSliderPrefab;
        public GameObject actNamePrefab;
        public GameObject actDescriptionPrefab;
        public GameObject actTimerPrefab;

        [Header("Objects")]
        protected PencilTool pencil;
        protected DrawableFrame drawableFrame;
        protected List<PaletteButton> paletteButtons;
        protected RadiusSlider radiusSlider;
        protected Text actName;
        protected Text actDescription;
        protected Timer actTimer;

        public override void ActInit() {
            base.ActInit();

            //Load settings
            DrawingSettings drawingSettings = settings as DrawingSettings;

            //Create drawable frame
            drawableFrame = Instantiate(drawableFramePrefab, transform).GetComponent<DrawableFrame>();
            drawableFrame.startingColor = drawingSettings.startingColor;

            //Init PencilTool
            pencil = drawableFrame.GetComponent<PencilTool>();
            try {
                pencil.selectedColor = drawingSettings.palette[0];
            } catch(ArgumentOutOfRangeException) {
                pencil.selectedColor = drawingSettings.startingColor;
            }

            //Init PaletteButtons
            paletteButtons = new List<PaletteButton>();
            for(int i = 0; i < drawingSettings.palette.Length; i++) {
                PaletteButton button = Instantiate(paletteButtonPrefab, canvas.transform).GetComponent<PaletteButton>();
                RectTransform btnTransform = button.GetComponent<RectTransform>();
                Image btnImage = button.GetComponent<Image>();
                button.pencil = pencil;
                btnImage.color = drawingSettings.palette[i];
                btnTransform.anchoredPosition = new Vector2(-420 + i * 110, 150);
                paletteButtons.Add(button);
            }

            //Init RadiusSlider
            radiusSlider = Instantiate(radiusSliderPrefab, canvas.transform).GetComponent<RadiusSlider>();
            radiusSlider.pencil = pencil;

            //Init actName Text
            actName = Instantiate(actNamePrefab, canvas.transform).GetComponent<Text>();
            actName.text = drawingSettings.actName;

            //Init actDescription Text
            actDescription = Instantiate(actDescriptionPrefab, canvas.transform).GetComponent<Text>();
            actDescription.text = drawingSettings.actDescription;

            //Init actTimer
            actTimer = Instantiate(actTimerPrefab, canvas.transform).GetComponent<Timer>();
            actTimer.TimerSet(drawingSettings.timeLimit);
        }

        public override void ActStart() {
            base.ActStart();

            //Unlock frame
            drawableFrame.interactable = false;

            //Start timer
            actTimer.OnTimeOut += new Action(ActEnd);
            actTimer.TimerStart();
        }

        public override void ActEnd() {
            base.ActEnd();

            //Lock frame
            drawableFrame.interactable = true;

            //Generate results
            results = new DrawingResults(drawableFrame.ToPNG());
        }
        
        public override void ActCleanup() {
            base.ActCleanup();

            Destroy(drawableFrame.gameObject);
            foreach(PaletteButton button in paletteButtons) {
                Destroy(button.gameObject);
            }
            Destroy(radiusSlider.gameObject);
            Destroy(actName.gameObject);
            Destroy(actDescription.gameObject);
            Destroy(actTimer.gameObject);
        }
    }

}