using Mirror;


namespace BetterTee.NetMsg 
{
    namespace Server 
    {
        namespace Error {
            public class InvalidPassword : MessageBase {}
            public class GameAlreadyStarted : MessageBase {}
        }

        public class LobbyStatusChange : MessageBase {
            public ConnectedPlayerData[] players;
            public ConnectedViewerData[] viewers;
        }

        public class LobbyEnd : MessageBase 
        {
            public ConnectedPlayerData[] players;
        }

        public class ActInit : MessageBase
        {
            public ActSettings settings;
        }
    
        public class ActStart : MessageBase {}

        public class ActEnd : MessageBase {}
        
        public class GameEnd : MessageBase
        {
            public ConnectedPlayerData[] leaderboard;
        }
    }

    namespace Client 
    {
        public class PlayerJoin : MessageBase
        {
            public string playerName;
            public string gamePassword;
        }

        public class ActResults : MessageBase
        {
            public ActResults results;
        }
    }

    namespace Viewer 
    {
        public class ViewerLink : MessageBase
        {
            public string viewerName;
            public string gamePassword;
        }

        public class Settings : MessageBase 
        {
            public GameSettings settings;
        }
    }
}