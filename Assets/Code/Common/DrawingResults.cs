using System;


[Serializable]
public class DrawingResults : ActResults {
    public readonly byte[] pngBytes;

    public DrawingResults(byte[] pngBytes) {
        this.pngBytes = pngBytes;
    }
}