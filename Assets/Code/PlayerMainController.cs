using System;
using UnityEngine;


public class PlayerMainController : MonoBehaviour
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
        ActSettings unknownSettings = JsonUtility.FromJson<ActSettings>(jsonData);

        if(unknownSettings.type == "Drawing") {
            currentAct = Instantiate(drawingControllerPrefab, transform).GetComponent<DrawingController>();
            currentAct.settings = JsonUtility.FromJson<DrawingSettings>(jsonData);
        }
        else if (unknownSettings.type == "Typing") {
            currentAct = Instantiate(typingControllerPrefab, transform).GetComponent<TypingController>();
            currentAct.settings = JsonUtility.FromJson<TypingSettings>(jsonData);
        }
        else {
            throw new InvalidJsonDataException(jsonData);
        }
    }
}
