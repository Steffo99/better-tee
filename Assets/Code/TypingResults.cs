using System;
using System.Collections.Generic;


[Serializable]
public class TypingResults : ActResults {
    public List<string> submissions;

    public TypingResults(List<string> submissions) {
        this.submissions = submissions;
    }
}