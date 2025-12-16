namespace Assets.Network
{
    public static class OnReceive
    {
        // --- Player Service ---
        public const string OnPlayerJoin = "OnPlayerJoin";
        public const string OnPlayerMove = "OnPlayerMove";

        // --- Entity Service ---
        public const string OnPlayerEntityJoin = "OnPlayerEntityJoin";
        public const string OnPlayerEntityMove = "OnPlayerEntityMove";
    }

    public static class OnSend
    {
        // --- Base ---
        public const string JoinGroup = "JoinGroup";

        // --- Player Service ---
        public const string PlayerJoin = "PlayerJoin";
        public const string PlayerMove = "PlayerMove";
    }
}
