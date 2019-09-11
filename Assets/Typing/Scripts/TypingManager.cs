using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[Serializable]
public class TypingSettings {
    public float timeLimit = 99f;
    public string actName = "Untitled";
    public string actDescription = "This Act is missing a description.";
}

public class TypingManager : MonoBehaviour
{
    [Header("Settings")]
    public string jsonString = "";
    public TypingSettings settings = null;

    [Header("Prefabs")]
    public GameObject actNamePrefab;
    public GameObject actDescriptionPrefab;
    public GameObject timerPrefab;
    public GameObject inputFieldPrefab;
    public GameObject submitPrefab;
    public GameObject submittedCountPrefab;

    [Header("Objects")]
    protected Text actName;
    protected Text actDescription;
    protected Timer timer;
    protected Canvas canvas;
    protected InputField inputField;
    protected Submit submit;
    protected Text submittedCount;
    protected EventSystem eventSystem;

    [Header("Results")]
    public List<string> texts;

    protected void Start() {
        if(jsonString != "") {
            JsonUtility.FromJsonOverwrite(jsonString, settings);
            if(settings == null) {
                Debug.LogWarning("Invalid settings json string, using defaults.");
            }
        }
        else {
            Debug.Log(JsonUtility.ToJson(settings));
        }

        texts = new List<string>();
        
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();

        actName = Instantiate(actNamePrefab, canvas.transform).GetComponent<Text>();
        actName.text = settings.actName;

        actDescription = Instantiate(actDescriptionPrefab, canvas.transform).GetComponent<Text>();
        actDescription.text = settings.actDescription;

        timer = Instantiate(timerPrefab, canvas.transform).GetComponent<Timer>();
        timer.TimerSet(settings.timeLimit);
        timer.OnTimeOut += ActEnd;

        inputField = Instantiate(inputFieldPrefab, canvas.transform).GetComponent<InputField>();
        Submit inputFieldSubmit = inputField.GetComponent<Submit>();
        inputFieldSubmit.typingManager = this;
        inputFieldSubmit.inputField = inputField;
        
        submit = Instantiate(submitPrefab, canvas.transform).GetComponent<Submit>();
        submit.typingManager = this;
        submit.inputField = inputField;

        submittedCount = Instantiate(submittedCountPrefab, canvas.transform).GetComponent<Text>();
        submittedCount.text = "";

        ActStart();
    }

    protected void ActStart() {
        timer.TimerStart();
        eventSystem.SetSelectedGameObject(inputField.gameObject);
        inputField.interactable = true;
        submit.GetComponent<Button>().interactable = true;
    }

    protected void ActEnd(object sender, EventArgs e) {
        inputField.interactable = false;
        submit.GetComponent<Button>().interactable = false;
    }

    public void SubmitText(string text) {
        if(text != "") {
            texts.Add(text);
            submittedCount.text = String.Format("Submitted: {0}", texts.Count);
        }
    }
}
