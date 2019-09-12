using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    public void NextAct() {
        //Get act data
        string jsonData = "";
        ActController.ActSettings unknownSettings = JsonUtility.FromJson<ActController.ActSettings>(jsonData);
    }

    public void SendActResults() {
        
    }
}
