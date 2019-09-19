using System;


[Serializable]
public class TypingResults : ActResults {
    public string[] submissions;

    public TypingResults(string[] submissions) {
        this.submissions = submissions;
    }
}