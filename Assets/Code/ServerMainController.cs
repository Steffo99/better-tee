using UnityEngine;
using System.Collections.Generic;
using Mirror;

public class ServerMainController : MonoBehaviour 
{
    public const int MAX_CONNECTIONS = 32;

    public bool isListening = false;

    public void ServerStart() {
        NetworkServer.Listen(MAX_CONNECTIONS);
        isListening = true;
    }
}
