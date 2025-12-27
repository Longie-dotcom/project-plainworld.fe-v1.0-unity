using Assets.UI.Enum;
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
        public bool ShowCustomizeCharacter { get; private set; }

        public bool ShowLobby { get; private set; }
        public bool ShowHUD { get; private set; }
        public bool ShowLoading { get; private set; }

        public event Action<UIState> OnUIStateChanged;
        public event Action<(PopUpType type, string message)> OnPopUpRequested;
        #endregion

        public UIState() { }

        #region Methods
        public void ShowPopUp(PopUpType type, string message)
        {
            OnPopUpRequested?.Invoke((type, message));
        }

        public void ApplyGameState(GameState game)
        {
            ShowLogin = game.Phase == GamePhase.Login;
            ShowRegister = game.Phase == GamePhase.Register;
            ShowCustomizeCharacter = game.Phase == GamePhase.CustomizeCharacter;

            ShowLoading = game.Phase == GamePhase.Loading;
            ShowHUD = game.Phase == GamePhase.InGame;

            OnUIStateChanged?.Invoke(this);
        }
        #endregion
    }
}
