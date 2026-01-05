using Assets.Service.Enum;
using Assets.State.Interface.IReadOnlyState;
using Assets.Utility;
using System;
using System.Collections.Generic;

namespace Assets.State
{
    public class GameState : IReadOnlyGameState
    {
        #region Attributes
        private readonly Stack<GamePhase> phaseStack = new();
        #endregion

        #region Properties
        public GamePhase Phase { get; private set; } = GamePhase.Paused;
        public GamePhase? PendingPhase { get; private set; }
        public bool IsLoading { get; private set; }

        public event Action<IReadOnlyGameState> OnRequestedNewScene;
        public event Action<IReadOnlyGameState> OnChangedPhase;
        #endregion

        public GameState() { }

        #region Methods
        public void RequestNewScene(GamePhase target)
        {
            if (Phase == target || IsLoading)
                return;

            PendingPhase = target;
            IsLoading = true;

            OnRequestedNewScene?.Invoke(this);
        }

        public void NotifySceneReady()
        {
            if (!IsLoading || PendingPhase == null)
                return;

            var next = PendingPhase.Value;

            PendingPhase = null;
            IsLoading = false;

            if (Phase == next)
                return;

            GameLogger.Info(
                Channel.Service,
                $"Phase committed: {Phase} to {next}");

            phaseStack.Clear();
            Phase = next;

            OnChangedPhase?.Invoke(this);
        }

        public void PushPhase(GamePhase overlay)
        {
            if (IsLoading)
            {
                GameLogger.Warning(
                    Channel.Service,
                    $"PushPhase ignored during loading: {overlay}");
                return;
            }

            if (Phase == overlay || phaseStack.Contains(overlay))
                return;

            phaseStack.Push(Phase);
            Phase = overlay;

            OnChangedPhase?.Invoke(this);
        }

        public void PopPhase()
        {
            if (IsLoading)
            {
                GameLogger.Warning(
                    Channel.Service,
                    $"PopPhase ignored during loading");
                return;
            }

            if (phaseStack.Count == 0)
                return;

            Phase = phaseStack.Pop();
            OnChangedPhase?.Invoke(this);
        }
        #endregion
    }
}
