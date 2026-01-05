using Assets.Service.Enum;
using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyGameState
    {
        GamePhase Phase { get; }
        GamePhase? PendingPhase { get; }
        bool IsLoading { get; }

        event Action<IReadOnlyGameState> OnRequestedNewScene;
        event Action<IReadOnlyGameState> OnChangedPhase;
    }
}
