using System;


[Serializable]
public class GameSettings
{
    public string gameName;
    public ActSettings[] acts;
    public int minimumPlayers = 0;
    public int maximumPlayers = 8;
}
