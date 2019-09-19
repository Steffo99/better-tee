using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace BetterTee.Server
{

    public class ServerMainController : MonoBehaviour 
    {

        [Header("Status")]
        public string lobbyPassword = null;
        public Dictionary<NetworkConnection, ConnectedPlayer> players;
        public Dictionary<NetworkConnection, ConnectedViewer> viewers;
        public GamePhase phase = GamePhase.UNINTIALIZED;
        public GameSettings gameSettings = null;
        public int? currentActNumber = null;

        public ActSettings CurrentActSettings {
            get {
                if(gameSettings == null) return null;
                if(currentActNumber == null) return null;
                return gameSettings.acts[currentActNumber.Value - 1];
            }
        }

        [Header("Constants")]
        public const int MAX_CONNECTIONS = 32;
        
        #region Unity Methods

        protected void Start() {
            ServerStart();
        }

        protected void OnDestroy() {
            if(NetworkServer.active) NetworkServer.Shutdown();
        }

        #endregion
        
        public void ServerStart() {
            LogFilter.Debug = true;
            phase = GamePhase.LOBBY;
            players = new Dictionary<NetworkConnection, ConnectedPlayer>();
            viewers = new Dictionary<NetworkConnection, ConnectedViewer>();
            Transport.activeTransport = GetComponent<TelepathyTransport>();

            #region Client Messages
            NetworkServer.RegisterHandler<NetMsg.Client.PlayerJoin>(OnPlayerJoin);
            NetworkServer.RegisterHandler<NetMsg.Client.ActResultsMsg>(OnActResults);
            #endregion

            #region Viewer Messages
            NetworkServer.RegisterHandler<NetMsg.Viewer.Settings>(OnGameSettings);
            NetworkServer.RegisterHandler<NetMsg.Viewer.ViewerLink>(OnViewerLink);
            NetworkServer.RegisterHandler<NetMsg.Viewer.GameStart>(OnGameStart);
            #endregion

            #region Other Messages
            NetworkServer.RegisterHandler<ConnectMessage>(OnConnect);
            NetworkServer.RegisterHandler<DisconnectMessage>(OnDisconnect);
            #endregion

            NetworkServer.Listen(MAX_CONNECTIONS);
        }

        public void SendLobbyUpdate() {
            SendToAllRegistered<NetMsg.Server.LobbyStatusChange>(new NetMsg.Server.LobbyStatusChange {
                players = players.Values.ToList().ConvertAll<ConnectedPlayerData>(player => player.Data).ToArray(),
                viewers = viewers.Values.ToList().ConvertAll<ConnectedViewerData>(viewer => viewer.Data).ToArray()
            });
        }

        public void GameStart() {
            phase = GamePhase.ACTS;
            currentActNumber = 1;      
        }

        public void ActInit() {
            SendToAllRegistered<NetMsg.Server.ActInit>(new NetMsg.Server.ActInit {
                settings = CurrentActSettings
            });
        }

        #region SendToAll Methods

        public void SendToAllPlayers<T>(T message, int channelId = 0)
        where T: IMessageBase {
            foreach(NetworkConnection connection in players.Keys) {
                connection.Send<T>(message, channelId);
            }
        }

        public void SendToAllViewers<T>(T message, int channelId = 0)
        where T: IMessageBase {
            foreach(NetworkConnection connection in viewers.Keys) {
                connection.Send<T>(message, channelId);
            }
        }

        public void SendToAllRegistered<T>(T message, int channelId = 0)
        where T: IMessageBase {
            SendToAllPlayers<T>(message, channelId);
            SendToAllViewers<T>(message, channelId);
        }

        #endregion

        #region Network Events

        protected void OnConnect(NetworkConnection connection, ConnectMessage message) {
            //Kick out clients that don't identify in 5 seconds?
        }

        protected void OnDisconnect(NetworkConnection connection, DisconnectMessage message) {
            //How to handle disconnections?
            try {
                players.Remove(connection);
            }
            catch(KeyNotFoundException) {}

            try {
                viewers.Remove(connection);
            }
            catch(KeyNotFoundException) {}

            SendLobbyUpdate();
        }

        protected void OnPlayerJoin(NetworkConnection connection, NetMsg.Client.PlayerJoin message)
        {
            if(message.gamePassword != lobbyPassword) {
                connection.Send<NetMsg.Server.Error.InvalidPassword>(new NetMsg.Server.Error.InvalidPassword());
                connection.Disconnect();
                return;
            }
            if(phase != GamePhase.LOBBY) {
                connection.Send<NetMsg.Server.Error.GameAlreadyStarted>(new NetMsg.Server.Error.GameAlreadyStarted());
                connection.Disconnect();
                return;
            }
            if(players.Count >= gameSettings.maximumPlayers) {
                connection.Send<NetMsg.Server.Error.MaxPlayersCapReached>(new NetMsg.Server.Error.MaxPlayersCapReached());
                connection.Disconnect();
                return;
            }

            ConnectedPlayer newPlayer = new ConnectedPlayer {
                name = message.playerName,
                id = players.Count
            };
            players.Add(connection, newPlayer);
            
            Debug.LogFormat("Player {0} joined the game", message.playerName);

            SendLobbyUpdate();
        }        

        protected void OnActResults(NetworkConnection connection, NetMsg.Client.ActResultsMsg message) {
            //Where should we put act results?
        }

        protected void OnGameSettings(NetworkConnection connection, NetMsg.Viewer.Settings message) {
            Debug.LogFormat("Received GameSettings from {0}", viewers[connection].name);
            gameSettings = message.settings;
        }

        protected void OnViewerLink(NetworkConnection connection, NetMsg.Viewer.ViewerLink message)
        {
            if(message.gamePassword != lobbyPassword) {
                connection.Send<NetMsg.Server.Error.InvalidPassword>(new NetMsg.Server.Error.InvalidPassword());
                connection.Disconnect();
                return;
            }

            ConnectedViewer newViewer = new ConnectedViewer {
                name = message.viewerName,
                id = viewers.Count
            };
            viewers.Add(connection, newViewer);

            Debug.LogFormat("Viewer {0} is now linked to the game", message.viewerName);

            SendLobbyUpdate();

        }

        protected void OnGameStart(NetworkConnection connection, NetMsg.Viewer.GameStart message) {
            
            if(gameSettings == null) {
                connection.Send<NetMsg.Server.Error.NoSettings>(new NetMsg.Server.Error.NoSettings());
                connection.Disconnect();
                return;
            }

            if(players.Count < gameSettings.minimumPlayers) {
                connection.Send<NetMsg.Server.Error.NotEnoughPlayers>(new NetMsg.Server.Error.NotEnoughPlayers());
                connection.Disconnect();
                return;
            }

            GameStart();
        }

        #endregion
    }

}