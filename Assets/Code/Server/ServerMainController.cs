using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace BetterTee.Server {

    public class ServerMainController : MonoBehaviour 
    {
        [Header("Status")]
        public string password = null;
        public List<ConnectedPlayerData> players;
        public List<ConnectedViewerData> viewers;
        public GamePhase phase = GamePhase.UNINTIALIZED;

        [Header("Constants")]
        public const int MAX_CONNECTIONS = 32;

        protected void Start() {
            StartServer();
        }

        protected void OnDestroy() {
            if(NetworkServer.active) NetworkServer.Shutdown();
        }

        public void StartServer() {
            LogFilter.Debug = true;
            phase = GamePhase.LOBBY;
            Transport.activeTransport = GetComponent<TelepathyTransport>();

            #region Client Messages
            NetworkServer.RegisterHandler<NetMsg.Client.PlayerJoin>(OnPlayerJoin);
            NetworkServer.RegisterHandler<NetMsg.Client.ActResults>(OnActResults);
            #endregion

            #region Viewer Messages
            NetworkServer.RegisterHandler<NetMsg.Viewer.ViewerLink>(OnViewerLink);
            #endregion

            #region Other Messages
            NetworkServer.RegisterHandler<ConnectMessage>(OnConnect);
            #endregion

            NetworkServer.Listen(MAX_CONNECTIONS);
        }

        #region Network Events

        protected void OnConnect(NetworkConnection connection, ConnectMessage message) {}

        protected void OnPlayerJoin(NetworkConnection connection, NetMsg.Client.PlayerJoin message)
        {
            if(message.gamePassword != password) {
                connection.Send<NetMsg.Server.Error.InvalidPassword>(new NetMsg.Server.Error.InvalidPassword());
                connection.Disconnect();
                return;
            }
            if(phase != GamePhase.LOBBY) {
                connection.Send<NetMsg.Server.Error.GameAlreadyStarted>(new NetMsg.Server.Error.GameAlreadyStarted());
                connection.Disconnect();
                return;
            }

            ConnectedPlayerData newPlayer = new ConnectedPlayerData {
                name = message.playerName,
                id = players.Count
            };
            players.Add(newPlayer);
            
            Debug.LogFormat("Player {0} joined the game", message.playerName);

            connection.Send<NetMsg.Server.LobbyStatusChange>(new NetMsg.Server.LobbyStatusChange {
                players = players.ToArray(),
                viewers = viewers.ToArray()
            });
        }        

        protected void OnActResults(NetworkConnection connection, NetMsg.Client.ActResults message) {
            //TODO
        }

        protected void OnViewerLink(NetworkConnection connection, NetMsg.Viewer.ViewerLink message)
        {
            if(message.gamePassword != password) {
                connection.Send<NetMsg.Server.Error.InvalidPassword>(new NetMsg.Server.Error.InvalidPassword());
                connection.Disconnect();
                return;
            }

            ConnectedViewerData newViewer = new ConnectedViewerData {
                id = viewers.Count
            };
            viewers.Add(newViewer);

            Debug.LogFormat("Viewer {0} is now linked to the game", message.viewerName);

            connection.Send<NetMsg.Server.LobbyStatusChange>(new NetMsg.Server.LobbyStatusChange {
                players = players.ToArray(),
                viewers = viewers.ToArray()
            });

        }

        #endregion
    }

}