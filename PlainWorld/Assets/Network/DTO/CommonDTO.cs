using System;

namespace Assets.Network.DTO
{
    [Serializable]
    public class HSVDTO
    {
        public float H { get; set; }
        public float S { get; set; }
        public float V { get; set; }
    }

    [Serializable]
    public class PlayerAppearance
    {
        public bool IsCreated { get; set; }

        public string HairID { get; set; }
        public string GlassesID { get; set; }
        public string ShirtID { get; set; }
        public string PantID { get; set; }
        public string ShoeID { get; set; }
        public string EyesID { get; set; }
        public string SkinID { get; set; }

        public HSVDTO HairColor { get; set; } = new HSVDTO();
        public HSVDTO PantColor { get; set; } = new HSVDTO();
        public HSVDTO EyeColor { get; set; } = new HSVDTO();
        public HSVDTO SkinColor { get; set; } = new HSVDTO();
    }

    [Serializable]
    public class PlayerMovement
    {
        public float MoveSpeed { get; set; }
        public PositionDTO Position { get; set; } = new PositionDTO();
        public PositionDTO CurrentDirection { get; set; } = new PositionDTO();
        public int CurrentAction { get; set; }
    }

    [Serializable]
    public class PositionDTO
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
