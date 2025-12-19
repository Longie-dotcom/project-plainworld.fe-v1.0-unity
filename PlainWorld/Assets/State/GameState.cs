using System;

namespace Assets.State
{
    public enum GamePhase
    {
        Login,
        Register,

        Connecting,
        Lobby,
        InGame,
        Paused,
        GameOver
    }

    public class GameState
    {
        #region Attributes
        #endregion

        #region Properties
        public GamePhase Phase { get; private set; }
        public bool IsLoading { get; private set; }

        public event Action<GameState> OnChanged;
        #endregion

        public GameState(GamePhase gamePhase)
        {
            Phase = gamePhase;
        }

        #region Methods
        public void SetPhase(GamePhase phase)
        {
            if (Phase == phase) return;

            Phase = phase;
            OnChanged?.Invoke(this);
        }
        #endregion
    }
}
