using Assets.Data.Enum;
using System;
using UnityEngine;

namespace Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent
{
    public interface IReadOnlyPlayerMovement
    {
        float MoveSpeed { get; }
        Vector2 Position { get; }
        Vector2 CurrentDirection { get; }
        EntityAction CurrentAction { get; }

        event Action<float> OnMoveSpeedChanged;
        event Action<Vector2> OnPositionChanged;
        event Action<Vector2> OnDirectionChanged;
        event Action<EntityAction> OnActionChanged;
    }
}
