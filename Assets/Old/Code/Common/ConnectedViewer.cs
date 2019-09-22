using System;


[Serializable]
public class ConnectedViewerData {
    public string name;
    public int id;
}

public class ConnectedViewer {
    public string name;
    public int id;

    public ConnectedViewerData Data {
        get {
            return new ConnectedViewerData {
                name = this.name,
                id = this.id
            };
        }
    }
}
