using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.State.UI;
using System;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class UIService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IUINetworkCommand UINetworkCommand { get; private set; }
        public UIState UIState { get; private set; } = new UIState();
        #endregion

        public UIService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (UINetworkCommand == null)
                throw new InvalidOperationException(
                    "UINetworkCommand not bound before Initialize");

            var gameService = ServiceLocator.Get<GameService>();
            gameService.GameState.OnChanged += UIState.ApplyGameState;

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            var gameService = ServiceLocator.Get<GameService>();
            gameService.GameState.OnChanged -= UIState.ApplyGameState;
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IUINetworkCommand command)
        {
            UINetworkCommand = command;
        }
        #endregion
    }
}
