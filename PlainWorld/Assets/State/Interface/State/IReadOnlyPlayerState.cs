using Assets.State.Interface.Component.Player;
using Assets.State.Interface.Component.Shared;
using System;

namespace Assets.State.Interface.State
{
    public interface IReadOnlyPlayerState
    {
        Guid PlayerID { get; }
        string PlayerName { get; }
        bool HasJoined { get; }

        IReadOnlyAct Act { get; }
        IReadOnlyPlayerAppearance Appearance { get; }

        event Action OnPlayerDataReady;
        event Action OnPlayerLogout;
        event Action OnPlayerForcedLogout;
    }
}
