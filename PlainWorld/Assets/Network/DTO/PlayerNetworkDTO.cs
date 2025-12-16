using System;

namespace Assets.Network.DTO
{
    public class PlayerJoinDTO
    {
        public Guid PlayerId { get; set; }
        public PositionDTO Position { get; set; }
    }

    public class PlayerMoveDTO
    {
        public Guid PlayerId { get; set; }
        public PositionDTO Position { get; set; }
    }

    public class PositionDTO
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
