using Telepathy;


namespace NetMessage 
{
    namespace Error 
    {
        public class InvalidPassword : MessageBase {}
    }

    namespace Connect 
    {
        public class PlayerJoin : MessageBase
        {
            public string playerName;
            public string gamePassword;
        }

        public class PlayerJoinSuccessful : MessageBase
        {
            public Player player;
        }

        public class ViewerLink : MessageBase
        {
            public string gamePassword;
        }

        public class ViewerLinkSuccessful : MessageBase 
        {
            public Viewer viewer;
        }

    }

    namespace Game 
    {
        public class Settings : MessageBase 
        {
            public GameSettings settings;
        }

        public class Start : MessageBase 
        {
            public Player[] players;
        }

        public class End : MessageBase
        {
            public Player[] leaderboard;
        }
    }

    namespace Act 
    {
        public class Init : MessageBase
        {
            public ActSettings settings;
        }

        public class Start : MessageBase {}

        public class Results : MessageBase
        {
            public ActResults results;
        }

        public class End : MessageBase {}
    }
}