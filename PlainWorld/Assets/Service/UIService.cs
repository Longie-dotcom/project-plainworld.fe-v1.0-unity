using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.Service.Interface;
using Assets.State;
using Assets.State.Interface.IReadOnlyState;
using Assets.UI.Enum;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class UIService : IService
    {
        #region Attributes
        private readonly UIState uiState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IUINetworkCommand UINetworkCommand { get; private set; }
        public IReadOnlyUIState UIState { get { return uiState; } }
        #endregion

        public UIService()
        { 
            uiState = new UIState();
        }

        #region Methods
        public Task InitializeAsync()
        {
            var gameService = ServiceLocator.Get<GameService>();
            gameService.GameState.OnChanged += uiState.ApplyGameState;

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            var gameService = ServiceLocator.Get<GameService>();
            gameService.GameState.OnChanged -= uiState.ApplyGameState;
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IUINetworkCommand command)
        {
            UINetworkCommand = command;
        }

        public void ShowPopUp(PopUpType type, string message)
        {
            uiState.ShowPopUp(type, message);
        }

        #region Senders
        #endregion

        #region Receivers
        #endregion
        #endregion
    }
}
