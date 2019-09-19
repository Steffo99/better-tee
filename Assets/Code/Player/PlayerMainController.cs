using System;
using UnityEngine;
using Mirror;
using BetterTee;

namespace BetterTee.Player 
{

    public class PlayerMainController : MonoBehaviour
    {
        [Header("WIP")]
        public string address = "127.0.0.1";
        public string playerName = "Steffo";
        public string gamePassword = "ASDF";

        void Start() {
            ConnectToServer(address, playerName);
        }

        [Header("Objects")]
        public ActController currentAct = null;

        [Header("Prefabs")]
        public GameObject drawingControllerPrefab;
        public GameObject typingControllerPrefab;

        [Serializable]
        public class InvalidActTypeException : Exception {
            public readonly string actType;

            public InvalidActTypeException(string actType) {
                this.actType = actType;
            }
        };

        public void LoadAct(ActSettings settings) {
            if(settings.type == "Drawing") {
                currentAct = Instantiate(drawingControllerPrefab, transform).GetComponent<DrawingController>();
            }
            else if (settings.type == "Typing") {
                currentAct = Instantiate(typingControllerPrefab, transform).GetComponent<TypingController>();
            }
            else throw new InvalidActTypeException(settings.type);
            currentAct.settings = settings;
        }

        public void ConnectToServer(string address, string playerName) {
            LogFilter.Debug = true;
            Transport.activeTransport = GetComponent<TelepathyTransport>();
            NetworkClient.RegisterHandler<ConnectMessage>(OnConnect);
            NetworkClient.RegisterHandler<NetMsg.Server.PlayerJoined>(OnPlayerJoinSuccessful);
            NetworkClient.RegisterHandler<NetMsg.Server.LobbyEnd>(OnLobbyEnd);
            NetworkClient.RegisterHandler<NetMsg.Server.GameEnd>(OnGameEnd);
            NetworkClient.RegisterHandler<NetMsg.Server.ActInit>(OnActInit);
            NetworkClient.RegisterHandler<NetMsg.Server.ActStart>(OnActStart);
            NetworkClient.RegisterHandler<NetMsg.Server.ActEnd>(OnActEnd);
            NetworkClient.Connect(address);
        }

        #region Network Events

        protected void OnConnect(NetworkConnection connection, ConnectMessage message) {
            Debug.Log("Sending NetMessage.Connect.PlayerJoin");
            connection.Send<NetMsg.Client.PlayerJoin>(new NetMsg.Client.PlayerJoin {
                playerName = playerName,
                gamePassword = gamePassword
            });
        }
        
        protected void OnPlayerJoinSuccessful(NetworkConnection connection, NetMsg.Server.PlayerJoined message) {

        }

        protected void OnLobbyEnd(NetworkConnection connection, NetMsg.Server.LobbyEnd message) {}

        protected void OnGameEnd(NetworkConnection connection, NetMsg.Server.GameEnd message) {}

        protected void OnActInit(NetworkConnection connection, NetMsg.Server.ActInit message) {
            LoadAct(message.settings);
            currentAct.ActInit();
        }

        protected void OnActStart(NetworkConnection connection, NetMsg.Server.ActStart message) {
            currentAct.ActStart();
        }

        protected void OnActEnd(NetworkConnection connection, NetMsg.Server.ActEnd message) {
            currentAct.ActEnd();
            //SEND RESULTS HERE

            //test this
            Destroy(currentAct);
        }

        #endregion

    }

}