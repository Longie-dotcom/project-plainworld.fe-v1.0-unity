using Assets.State.Component.Player;
using Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent;
using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyPlayerState
    {
        Guid PlayerID { get; }
        string PlayerName { get; }
        bool HasJoined { get; }

        IReadOnlyPlayerMovement Movement { get; }
        IReadOnlyPlayerAppearance Appearance { get; }

        event Action OnPlayerReady;
        event Action OnPlayerLogout;
        event Action OnPlayerForcedLogout;
        event Action<PlayerAppearance> OnPlayerCustomization;
    }
}
