using System;

namespace Assets.State
{
    public class UIState 
    {
        #region Attributes
        #endregion

        #region Properites
        public bool ShowLogin { get; private set; }
        public bool ShowRegister { get; private set; }

        public bool ShowLobby { get; private set; }
        public bool ShowHUD { get; private set; }
        public bool ShowLoading { get; private set; }

        public event Action<UIState> OnUIStateChanged;
        #endregion

        public UIState() { }

        #region Methods
        public void ApplyGameState(GameState game)
        {
            ShowLogin = game.Phase == GamePhase.Login;
            ShowRegister = game.Phase == GamePhase.Register;

            ShowLoading = game.Phase == GamePhase.Connecting;
            ShowLobby = game.Phase == GamePhase.Lobby;
            ShowHUD = game.Phase == GamePhase.InGame;

            OnUIStateChanged?.Invoke(this);
        }
        #endregion
    }
}
