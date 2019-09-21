using System;
using UnityEngine;
using Mirror;


namespace BetterTee.Viewer 
{
        
    public class ViewerMainController : MonoBehaviour
    {
        [Header("WIP")]
        public string address = "127.0.0.1";
        public string viewerName = "Unknown";
        public string gamePassword = "ASDF";

        void Start() {
            viewerName = Environment.MachineName;
            ConnectToServer(address, viewerName);
        }

        [Header("Objects")]
        public ActViewer currentAct = null;
        public LobbyController lobbyController = null;

        [Header("Prefabs")]
        public GameObject lobbyControllerPrefab = null;
        public GameObject drawingViewerPrefab = null;
        public GameObject typingViewerPrefab = null;

        [Serializable]
        public class InvalidActTypeException : Exception {
            public readonly string actType;

            public InvalidActTypeException(string actType) {
                this.actType = actType;
            }
        };

        public void LoadAct(ActSettings settings) {
            throw new InvalidActTypeException(settings.type);
        }

        public void ConnectToServer(string address, string playerName) {
            LogFilter.Debug = true;
            Transport.activeTransport = GetComponent<TelepathyTransport>();
            NetworkClient.RegisterHandler<ConnectMessage>(OnConnect);
            NetworkClient.RegisterHandler<DisconnectMessage>(OnDisconnect);
            NetworkClient.RegisterHandler<NetMsg.Server.LobbyStatusChange>(OnLobbyStatusChange);
            NetworkClient.RegisterHandler<NetMsg.Server.LobbyEnd>(OnLobbyEnd);
            NetworkClient.RegisterHandler<NetMsg.Server.GameEnd>(OnGameEnd);
            NetworkClient.RegisterHandler<NetMsg.Server.ActInit>(OnActInit);
            NetworkClient.RegisterHandler<NetMsg.Server.ActStart>(OnActStart);
            NetworkClient.RegisterHandler<NetMsg.Server.ActEnd>(OnActEnd);
            NetworkClient.Connect(address);
        }

        public void StartGame() {
            NetworkClient.Send<NetMsg.Viewer.GameStart>(new NetMsg.Viewer.GameStart {});
        }

        #region Network Events

        protected void OnConnect(NetworkConnection connection, ConnectMessage message) {
            Debug.Log("Sending ViewerLink message");
            connection.Send<NetMsg.Viewer.ViewerLink>(new NetMsg.Viewer.ViewerLink {
                viewerName = viewerName,
                gamePassword = gamePassword
            });

            lobbyController = Instantiate(lobbyControllerPrefab, transform).GetComponent<LobbyController>();
            lobbyController.startGameAction = StartGame;
        }

        protected void OnDisconnect(NetworkConnection connection, DisconnectMessage message) {
            Debug.LogWarning("Lost connection");
        }
        
        protected void OnLobbyStatusChange(NetworkConnection connection, NetMsg.Server.LobbyStatusChange message) {
            lobbyController.OnLobbyStatusChange(message.players, message.viewers);
        }

        protected void OnLobbyEnd(NetworkConnection connection, NetMsg.Server.LobbyEnd message) {}

        protected void OnGameEnd(NetworkConnection connection, NetMsg.Server.GameEnd message) {}

        protected void OnActInit(NetworkConnection connection, NetMsg.Server.ActInit message) {
            LoadAct(message.settings);
            //currentAct.ActInit();
        }

        protected void OnActStart(NetworkConnection connection, NetMsg.Server.ActStart message) {
            //currentAct.ActStart();
        }

        protected void OnActEnd(NetworkConnection connection, NetMsg.Server.ActEnd message) {
            //currentAct.ActEnd();
            //Destroy(currentAct);
        }

        #endregion

    }

}