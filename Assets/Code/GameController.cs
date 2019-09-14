using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    [Header("TEST")]
    public string jsonData = "";
    
    protected void Start() {
        Debug.Log("Testing ActInit with public jsonData...");
        LoadAct(jsonData);
    }

    [Header("Objects")]
    public ActController currentAct;

    [Header("Prefabs")]
    public GameObject drawingControllerPrefab;
    public GameObject typingControllerPrefab;

    [Serializable]
    public class InvalidJsonDataException : Exception {
        public readonly string jsonData;

        public InvalidJsonDataException(string jsonData) {
            this.jsonData = jsonData;
        }
    };

    public void LoadAct(string jsonData) {
        ActController.ActSettings unknownSettings = JsonUtility.FromJson<ActController.ActSettings>(jsonData);

        if(unknownSettings.type == "Drawing") {
            currentAct = Instantiate(drawingControllerPrefab, transform).GetComponent<DrawingController>();
            currentAct.settings = JsonUtility.FromJson<DrawingController.DrawingSettings>(jsonData);
        }
        else if (unknownSettings.type == "Typing") {
            currentAct = Instantiate(typingControllerPrefab, transform).GetComponent<TypingController>();
            currentAct.settings = JsonUtility.FromJson<TypingController.TypingSettings>(jsonData);
        }
        else {
            throw new InvalidJsonDataException(jsonData);
        }
    }
}
