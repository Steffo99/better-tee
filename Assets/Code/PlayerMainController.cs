using System;
using UnityEngine;
using Mirror;


public class PlayerMainController : MonoBehaviour
{
    [Header("WIP")]
    public string address = "127.0.0.1:44444";
    public string playerName = "Steffo";

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
        NetworkClient.RegisterHandler<NetMessage.Connect.PlayerJoinSuccessful>(OnPlayerJoinSuccessful);
        NetworkClient.RegisterHandler<NetMessage.Game.Settings>(OnGameSettings);
        NetworkClient.RegisterHandler<NetMessage.Game.Start>(OnGameStart);
        NetworkClient.RegisterHandler<NetMessage.Game.End>(OnGameEnd);
        NetworkClient.RegisterHandler<NetMessage.Act.Init>(OnActInit);
        NetworkClient.RegisterHandler<NetMessage.Act.Start>(OnActStart);
        NetworkClient.RegisterHandler<NetMessage.Act.End>(OnActEnd);
        NetworkClient.Connect(address);

        NetMessage.Connect.PlayerJoin playerJoin = new NetMessage.Connect.PlayerJoin {
            playerName = playerName
        };
        NetworkClient.Send<NetMessage.Connect.PlayerJoin>(playerJoin);
    }
    
    protected void OnPlayerJoinSuccessful(NetworkConnection connection, NetMessage.Connect.PlayerJoinSuccessful message) {}

    protected void OnGameSettings(NetworkConnection connection, NetMessage.Game.Settings message) {}

    protected void OnGameStart(NetworkConnection connection, NetMessage.Game.Start message) {}

    protected void OnGameEnd(NetworkConnection connection, NetMessage.Game.End message) {}

    protected void OnActInit(NetworkConnection connection, NetMessage.Act.Init message) {
        LoadAct(message.settings);
        currentAct.ActInit();
    }

    protected void OnActStart(NetworkConnection connection, NetMessage.Act.Start message) {
        currentAct.ActStart();
    }

    protected void OnActEnd(NetworkConnection connection, NetMessage.Act.End message) {
        currentAct.ActEnd();
        //SEND RESULTS HERE

        //test this
        Destroy(currentAct);
    }

}
