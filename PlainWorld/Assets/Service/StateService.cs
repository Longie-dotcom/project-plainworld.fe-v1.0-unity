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
        private readonly PlayerState playerState = new();
        private readonly UIState uiState = new();
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IStateNetworkCommand StateNetworkCommand { get; private set; }

        public event Action<GameState> OnGameChanged;
        public event Action<PlayerState> OnPlayerChanged;
        public event Action<UIState> OnUIChanged;
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
            RecalculateUI();
        }

        public void SetPlayerPosition(float x, float y)
        {
            playerState.X = x;
            playerState.Y = y;
            OnPlayerChanged?.Invoke(playerState);
        }

        private void RecalculateUI()
        {
            uiState.ShowLoading = gameState.Phase == GamePhase.Connecting;
            uiState.ShowLobby = gameState.Phase == GamePhase.Lobby;
            uiState.ShowHUD = gameState.Phase == GamePhase.InGame;

            OnUIChanged?.Invoke(uiState);
        }
        #endregion
    }
}
