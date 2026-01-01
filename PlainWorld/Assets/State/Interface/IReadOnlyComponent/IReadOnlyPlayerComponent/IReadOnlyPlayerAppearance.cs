using System;
using UnityEngine;

namespace Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent
{
    public interface IReadOnlyPlayerAppearance
    {
        bool IsCreated { get; }

        string HairID { get; }
        string GlassesID { get; }
        string ShirtID { get; }
        string PantID { get; }
        string ShoeID { get; }
        string EyesID { get; }
        string SkinID { get; }

        Color HairColor { get; }
        Color PantColor { get; }
        Color EyeColor { get; }
        Color SkinColor { get; }

        event Action OnChanged;
    }
}
