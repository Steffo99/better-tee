using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace BetterTee.Viewer 
{
    
    public class LobbyController : MonoBehaviour
    {
        [Header("Objects")]
        public Canvas canvas = null;
        public EventSystem eventSystem = null;
        public Action startGameAction = null;
        public Text lobbyText = null;
        public InputField gameSettingsField = null;
        public StartGameBtn startGameBtn = null;

        [Header("Prefabs")]
        public GameObject lobbyTextPrefab;
        public GameObject gameSettingsFieldPrefab;
        public GameObject startGameBtnPrefab;
        
        protected void Start() {
            canvas = GameObject.FindGameObjectWithTag("Canvas")?.GetComponent<Canvas>();
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem")?.GetComponent<EventSystem>();

            lobbyText = Instantiate(lobbyTextPrefab, canvas.transform).GetComponent<Text>();
            gameSettingsField = Instantiate(gameSettingsFieldPrefab, canvas.transform).GetComponent<InputField>();
            startGameBtn = Instantiate(startGameBtnPrefab, canvas.transform).GetComponent<StartGameBtn>();
            startGameBtn.lobbyController = this;
        }

        public void OnLobbyStatusChange(ConnectedPlayerData[] players, ConnectedViewerData[] viewers, bool canStart) {
            gameSettingsField.interactable = true;
            startGameBtn.GetComponent<Button>().interactable = canStart;    
        }

        public void OnStartGameBtnPress() {
            startGameAction();
        }

        protected void OnDestroy() {
            Destroy(lobbyText.gameObject);
            Destroy(gameSettingsField.gameObject);
            Destroy(startGameBtn.gameObject);
        }
    }
}
