using Assets.Service.Enum;
using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyGameState
    {
        GamePhase Phase { get; }
        bool IsLoading { get; }

        event Action<GameState> OnChanged;
    }
}
