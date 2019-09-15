using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class ServerMainController : MonoBehaviour 
{
    [Header("Status")]
    public bool isListening = false;

    [Header("Constants")]
    public const int MAX_CONNECTIONS = 32;

    public void ServerStart() {
        NetworkServer.Listen(MAX_CONNECTIONS);
        NetworkServer.RegisterHandler<NetMessages.PlayerConnectionMessage>(OnPlayerConnect);
        NetworkServer.RegisterHandler<NetMessages.ViewerConnectionMessage>(OnViewerConnect);
        NetworkServer.RegisterHandler<NetMessages.ActResultsMessage>(OnActResults);
        isListening = true;
    }

    public void OnPlayerConnect(NetworkConnection connection, NetMessages.PlayerConnectionMessage message) {}
    public void OnViewerConnect(NetworkConnection connection, NetMessages.ViewerConnectionMessage message) {}
    public void OnActResults(NetworkConnection connection, NetMessages.ActResultsMessage message) {}   
}
