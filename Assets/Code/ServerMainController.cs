using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ServerMainController : MonoBehaviour 
{
    [Header("Status")]
    public string password = null;
    public List<Player> players;
    public List<Viewer> viewers;
    public GamePhase phase = GamePhase.UNINTIALIZED;

    [Header("Constants")]
    public const int MAX_CONNECTIONS = 32;

    public void ServerStart() {
        phase = GamePhase.LOBBY;
        NetworkServer.RegisterHandler<NetMessage.Connect.PlayerJoin>(OnPlayerJoin);
        NetworkServer.RegisterHandler<NetMessage.Connect.ViewerLink>(OnViewerLink);
        NetworkServer.RegisterHandler<NetMessage.Game.Settings>(OnGameSettings);
        NetworkServer.RegisterHandler<NetMessage.Act.Results>(OnActResults);
        NetworkServer.Listen(MAX_CONNECTIONS);
    }

    public void OnPlayerJoin(NetworkConnection connection, NetMessage.Connect.PlayerJoin message) {
        if(message.gamePassword != password) {
            connection.Send<NetMessage.Error.InvalidPassword>(new NetMessage.Error.InvalidPassword());
            return;
        }
        Player newPlayer = new Player(message.playerName, new Guid());
        players.Add(newPlayer);

        NetMessage.Connect.PlayerJoinSuccessful reply = new NetMessage.Connect.PlayerJoinSuccessful {
            player = newPlayer
        };
        connection.Send<NetMessage.Connect.PlayerJoinSuccessful>(reply);
    } 

    public void OnViewerLink(NetworkConnection connection, NetMessage.Connect.ViewerLink message) {
        if(message.gamePassword != password) {
            connection.Send<NetMessage.Error.InvalidPassword>(new NetMessage.Error.InvalidPassword());
            return;
        }
        Viewer newViewer = new Viewer(new Guid());
        viewers.Add(newViewer);

        NetMessage.Connect.ViewerLinkSuccessful reply = new NetMessage.Connect.ViewerLinkSuccessful {
            viewer = newViewer
        };
        connection.Send<NetMessage.Connect.ViewerLinkSuccessful>(reply);
    } 

    public void OnGameSettings(NetworkConnection connection, NetMessage.Game.Settings message) {} 
    public void OnActResults(NetworkConnection connection, NetMessage.Act.Results message) {}
}
