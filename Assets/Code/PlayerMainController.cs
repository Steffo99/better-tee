using System;
using UnityEngine;
using Mirror;


public class PlayerMainController : MonoBehaviour
{
    [Header("Objects")]
    public ActController currentAct;

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

    public void ConnectToServer(string address) {
        NetworkClient.Connect(address);
        NetworkClient.RegisterHandler<NetMessages.ConnectionSuccessfulResponse>(OnConnectionSuccessful);
        NetworkClient.RegisterHandler<NetMessages.GameStartMessage>(OnGameStart);
        NetworkClient.RegisterHandler<NetMessages.ActSettingsMessage>(OnActSettings);
        NetworkClient.RegisterHandler<NetMessages.ActEndNotification>(OnActEnd);
    }

    public void OnConnectionSuccessful(NetworkConnection connection, NetMessages.ConnectionSuccessfulResponse message) {}
    public void OnGameStart(NetworkConnection connection, NetMessages.GameStartMessage message) {}
    public void OnActEnd(NetworkConnection connection, NetMessages.ActEndNotification message) {}
    
    public void OnActSettings(NetworkConnection connection, NetMessages.ActSettingsMessage message) {
        LoadAct(message.settings);
    }

}
