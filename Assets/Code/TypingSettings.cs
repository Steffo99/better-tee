using System;


[Serializable]
public class TypingSettings : ActSettings {
    public float timeLimit = 99f;
    public string actName = "Untitled";
    public string actDescription = "This Act is missing a description.";
    public string destinationPool = "default";

    public TypingSettings(float timeLimit, string actName, string actDescription, string destinationPool) {
        this.type = "Typing";
        this.timeLimit = timeLimit;
        this.actName = actName;
        this.actDescription = actDescription;
        this.destinationPool = destinationPool;
    }    
}