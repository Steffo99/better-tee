using System;

[Serializable]
public struct Player {
    public string name;
    public Guid guid;

    public Player(string name, Guid guid) {
        this.name = name;
        this.guid = guid;
    }
}
