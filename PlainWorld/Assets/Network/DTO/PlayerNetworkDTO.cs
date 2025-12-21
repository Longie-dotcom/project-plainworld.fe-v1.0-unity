using System;

namespace Assets.Network.DTO
{
    // Request DTO
    public class PlayerJoinDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class PlayerLogoutDTO
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class PlayerMoveDTO
    {
        public Guid ID { get; set; }
        public PositionDTO Position { get; set; } = new PositionDTO();
    }


    // Response DTO
    public class PlayerDTO
    {
        public Guid ID { get; set; } = Guid.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
        public PositionDTO Position { get; set; } = new PositionDTO();
    }

    public class PlayerEntityDTO
    {
        public Guid ID { get; set; } = Guid.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public PositionDTO Position { get; set; } = new PositionDTO();
    }

    public class PlayerPositionDTO
    {
        public Guid ID { get; set; } = Guid.Empty;
        public PositionDTO Position { get; set; } = new PositionDTO();
    }

    public class PlayerEntityPositionDTO
    {
        public Guid ID { get; set; } = Guid.Empty;
        public PositionDTO Position { get; set; } = new PositionDTO();
    }

    public class PositionDTO
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
