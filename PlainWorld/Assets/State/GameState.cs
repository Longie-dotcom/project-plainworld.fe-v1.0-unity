using Assets.Utility;
using System;
using System.Collections.Generic;

namespace Assets.State
{
    public enum GamePhase
    {
        // --- UI States ---
        Login,
        Register,
        CustomizeCharacter,
        Loading,
        InGame,

        Paused,
        GameOver,
    }

    public class GameState
    {
        #region Attributes
        private readonly Stack<GamePhase> phaseStack = new();
        #endregion

        #region Properties
        public GamePhase Phase { get; private set; }
        public bool IsLoading { get; private set; }

        public event Action<GameState> OnChanged;
        #endregion

        public GameState(GamePhase initialPhase)
        {
            Phase = initialPhase;
        }

        #region Methods
        public void SetPhase(GamePhase next)
        {
            if (Phase == next)
                return;

            GameLogger.Info(
                Channel.Service,
                $"Phase change: {Phase} to {next}");

            phaseStack.Clear();
            Phase = next;

            OnChanged?.Invoke(this);
        }

        public void PushPhase(GamePhase overlay)
        {
            GameLogger.Info(
                Channel.Service,
                $"Push phase: {Phase} to {overlay}");

            phaseStack.Push(Phase);
            Phase = overlay;

            OnChanged?.Invoke(this);
        }

        public void PopPhase()
        {
            if (phaseStack.Count == 0)
                return;

            var previous = phaseStack.Pop();

            GameLogger.Info(
                Channel.Service,
                $"Pop phase: {Phase} to {previous}");

            Phase = previous;

            OnChanged?.Invoke(this);
        }
        #endregion
    }
}
