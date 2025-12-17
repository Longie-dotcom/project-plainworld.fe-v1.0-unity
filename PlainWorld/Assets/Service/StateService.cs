using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.State;
using System;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class StateService : IService
    {
        #region Attributes
        private readonly GameState gameState = new();
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IStateNetworkCommand StateNetworkCommand { get; private set; }

        public event Action<GameState> OnGameChanged;
        #endregion

        public StateService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (StateNetworkCommand == null)
                throw new InvalidOperationException(
                    "StateNetworkCommand not bound before Initialize");

            SetGamePhase(GamePhase.Connecting);

            IsInitialized = true;

            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IStateNetworkCommand command)
        {
            StateNetworkCommand = command;
        }

        public void SetGamePhase(GamePhase phase)
        {
            gameState.Phase = phase;
            OnGameChanged?.Invoke(gameState);
        }
        #endregion
    }
}
