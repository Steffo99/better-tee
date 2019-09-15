using System;
using Mirror;


namespace NetMessages {

    public class ErrorMessage : MessageBase
    {
        public string errorName;
        public string errorDescription = "";
    }

    public class PlayerConnectionMessage : MessageBase
    {
        public string playerName;
        public string gamePassword;
    }

    public class ViewerConnectionMessage : MessageBase
    {
        public string gamePassword;
    }

    public class ConnectionSuccessfulResponse : MessageBase
    {
        
    }

    public class GameStartMessage : MessageBase
    {
        
    }

    public class ActSettingsMessage : MessageBase
    {
        public ActSettings settings;
    }

    public class ActResultsMessage : MessageBase
    {
        public ActResults results;
    }

    public class ActEndNotification : MessageBase 
    {

    }
}