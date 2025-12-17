using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.State;
using System;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class UIService : IService
    {
        #region Attributes
        private StateService state;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IUINetworkCommand UINetworkCommand { get; private set; }

        public event Action<UIState> OnUIStateChanged;
        #endregion

        public UIService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (UINetworkCommand == null)
                throw new InvalidOperationException(
                    "UINetworkCommand not bound before Initialize");

            state = ServiceLocator.Get<StateService>();
            state.OnGameChanged += HandleGameState;

            IsInitialized = true;

            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IUINetworkCommand command)
        {
            UINetworkCommand = command;
        }

        private void HandleGameState(GameState game)
        {
            var ui = new UIState
            {
                ShowLogin = game.Phase == GamePhase.None,
                ShowLoading = game.Phase == GamePhase.Connecting,
                ShowLobby = game.Phase == GamePhase.Lobby,
                ShowHUD = game.Phase == GamePhase.InGame
            };

            OnUIStateChanged?.Invoke(ui);
        }
        #endregion
    }
}
