using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BetterTee.Viewer 
{
        
    public class StartGameBtn : MonoBehaviour
    {
        public LobbyController lobbyController;

        public void OnClick() {
            lobbyController.OnStartGameBtnPress();
        }
    }

}