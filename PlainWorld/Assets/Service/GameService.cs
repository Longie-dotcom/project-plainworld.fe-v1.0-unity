using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.State;
using System;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class GameService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IGameNetworkCommand StateNetworkCommand { get; private set; }
        public GameState GameState { get; private set; } = new GameState(GamePhase.Loading);
        #endregion

        public GameService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (StateNetworkCommand == null)
                throw new InvalidOperationException(
                    "StateNetworkCommand not bound before Initialize");

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IGameNetworkCommand command)
        {
            StateNetworkCommand = command;
        }
        #endregion
    }
}
