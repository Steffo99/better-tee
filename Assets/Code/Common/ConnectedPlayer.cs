using System;


[Serializable]
public class ConnectedPlayerData {
    public string name;
    public int id;
}

public class ConnectedPlayer {
    public string name;
    public int id;

    public ConnectedPlayerData Data {
        get {
            return new ConnectedPlayerData {
                name = this.name,
                id = this.id
            };
        }
    }
}