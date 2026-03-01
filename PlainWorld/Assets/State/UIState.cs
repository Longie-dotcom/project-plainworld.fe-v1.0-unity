using Assets.Service.Enum;
using Assets.State.Interface.State;
using Assets.UI.Enum;
using System;

namespace Assets.State
{
    public class UIState : IReadOnlyUIState
    {
        #region Attributes
        #endregion

        #region Properites
        public bool ShowMenu { get; private set; }
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

        public void ApplyGameState(IReadOnlyGameState game)
        {
            ShowMenu = game.Phase == GamePhase.Menu;
            ShowHUD = game.Phase == GamePhase.InGame;
            ShowLoading = game.Phase == GamePhase.Loading;

            OnUIStateChanged?.Invoke(this);
        }
        #endregion
    }
}
