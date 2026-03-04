using Assets.State.Interface.Component.Player;
using Assets.State.Interface.Component.Shared;
using System;
using UnityEngine;

namespace Assets.State.Interface.State
{
    public interface IReadOnlyPlayerState
    {
        Guid PlayerID { get; }
        string PlayerName { get; }
        bool HasJoined { get; }

        IReadOnlyAct Act { get; }
        IReadOnlyPlayerAppearance Appearance { get; }
        IReadOnlyInventory Inventory { get; }

        event Action OnPlayerDataReady;
        event Action OnPlayerLogout;
        event Action OnPlayerForcedLogout;

        event Action<Vector2, string> OnWorldObjectPlaced;
    }
}
