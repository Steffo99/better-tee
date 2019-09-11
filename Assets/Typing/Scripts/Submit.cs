using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Submit : MonoBehaviour
{
    public InputField inputField;
    public TypingManager typingManager;

    protected EventSystem eventSystem;

    protected void Start() {
        eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
    }

    public void OnClick() {
        typingManager.SubmitText(inputField.text);
        inputField.text = "";
        eventSystem.SetSelectedGameObject(inputField.gameObject);
    }
}
