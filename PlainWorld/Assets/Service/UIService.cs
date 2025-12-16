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
            state.OnUIChanged += HandleUIState;

            IsInitialized = true;

            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            state.OnUIChanged -= HandleUIState;
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IUINetworkCommand command)
        {
            UINetworkCommand = command;
        }

        private void HandleUIState(UIState ui)
        {
            OnUIStateChanged?.Invoke(ui);
        }
        #endregion
    }
}
