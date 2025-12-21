namespace Assets.Network
{
    public static class OnReceive
    {
        // --- Player Service ---
        public const string OnPlayerJoin = "OnPlayerJoin";
        public const string OnPlayerLogout = "OnPlayerLogout";
        public const string OnPlayerMove = "OnPlayerMove";

        // --- Entity Service ---
        public const string OnPlayerEntityJoin = "OnPlayerEntityJoin";
        public const string OnPlayerEntityLogout = "OnPlayerEntityLogout";
        public const string OnPlayerEntityMove = "OnPlayerEntityMove";
        public const string OnPlayerEntityOnline = "OnPlayerEntityOnline";
    }

    public static class OnSend
    {
        // --- Base ---
        public const string JoinGroup = "JoinGroup";
        public const string LeaveGroup = "LeaveGroup";
        
        // --- Player Service ---
        public const string PlayerJoin = "PlayerJoin";
        public const string PlayerMove = "PlayerMove";
        public const string PlayerLogout = "PlayerLogout";
    }
}
