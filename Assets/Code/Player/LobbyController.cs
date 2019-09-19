using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace BetterTee.Player 
{
    
    public class LobbyController : MonoBehaviour
    {
        [Header("Objects")]
        public Canvas canvas = null;
        public EventSystem eventSystem = null;
        public Text lobbyText;
        public Text playersText;
        public Text viewersText;
        public Text playersList;
        public Text viewersList;

        [Header("Prefabs")]
        public GameObject lobbyTextPrefab;
        public GameObject playersTextPrefab;
        public GameObject viewersTextPrefab;
        public GameObject playersListPrefab;
        public GameObject viewersListPrefab;
        
        protected void Start() {
            canvas = GameObject.FindGameObjectWithTag("Canvas")?.GetComponent<Canvas>();
            eventSystem = GameObject.FindGameObjectWithTag("EventSystem")?.GetComponent<EventSystem>();

            lobbyText = Instantiate(lobbyTextPrefab, canvas.transform).GetComponent<Text>();
            playersText = Instantiate(playersTextPrefab, canvas.transform).GetComponent<Text>();
            viewersText = Instantiate(viewersTextPrefab, canvas.transform).GetComponent<Text>();
            playersList = Instantiate(playersListPrefab, canvas.transform).GetComponent<Text>();
            viewersList = Instantiate(viewersListPrefab, canvas.transform).GetComponent<Text>();
        }

        public void OnLobbyStatusChange(ConnectedPlayerData[] players, ConnectedViewerData[] viewers) {
            playersList.text = "";
            viewersList.text = "";

            foreach(ConnectedPlayerData player in players) {
                playersList.text += String.Format("[{0}] {1}\n", player.id, player.name);
            }
            
            foreach(ConnectedViewerData viewer in viewers) {
                viewersList.text += String.Format("[{0}] {1}\n", viewer.id, viewer.name);
            }
        }

        protected void OnDestroy() {
            Destroy(lobbyText.gameObject);
            Destroy(playersText.gameObject);
            Destroy(viewersText.gameObject);
            Destroy(playersList.gameObject);
            Destroy(viewersList.gameObject);
        }
    }
}
