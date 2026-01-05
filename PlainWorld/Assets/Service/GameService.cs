using Assets.Core;
using Assets.Network;
using Assets.Network.Interface.Command;
using Assets.Service.Enum;
using Assets.Service.Interface;
using Assets.State;
using Assets.State.Interface.IReadOnlyState;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class GameService : IService
    {
        #region Attributes
        private readonly GameState gameState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IGameNetworkCommand GameNetworkCommand { get; private set; }
        public IReadOnlyGameState GameState { get { return gameState; } }
        #endregion

        public GameService()
        {
            gameState = new GameState();
        }

        #region Methods
        public Task InitializeAsync()
        {
            var playerService = ServiceLocator.Get<PlayerService>();
            playerService.PlayerState.OnPlayerDataReady += PlayerReady;

            var uiService = ServiceLocator.Get<UIService>();
            GameState.OnChangedPhase += uiService.ApplyGameState;

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            var playerService = ServiceLocator.Get<PlayerService>();
            playerService.PlayerState.OnPlayerDataReady -= PlayerReady;

            var uiService = ServiceLocator.Get<UIService>();
            GameState.OnChangedPhase -= uiService.ApplyGameState;

            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IGameNetworkCommand command)
        {
            GameNetworkCommand = command;
        }

        public void NotifySceneReady()
        {
            gameState.NotifySceneReady();
        }

        public void PushPhase(GamePhase overlay)
        {
            gameState.PushPhase(overlay);
        }

        public void PopPhase()
        {
            gameState.PopPhase();
        }

        #region App Life-cycle
        public void StartGame()
        {
            gameState.RequestNewScene(GamePhase.Login);
        }
        #endregion

        #region Player Life-cycle
        public async Task PlayerLogin(string email, string password)
        {
            var authService = ServiceLocator.Get<AuthService>();
            var networkService = ServiceLocator.Get<NetworkService>();
            var playerService = ServiceLocator.Get<PlayerService>();

            // Authenticate players
            await authService.Login(email, password);

            // Connect to the game service and start session
            await networkService.ConnectAsync(authService.AuthState.Token);
            await networkService.Session.StartSessionAsync();

            // Request spawning players
            await playerService.JoinAsync();
        }

        public void PlayerReady()
        {
            gameState.RequestNewScene(GamePhase.InGame);
        }

        public async Task PlayerLogout()
        {
            var networkService = ServiceLocator.Get<NetworkService>();
            var playerService = ServiceLocator.Get<PlayerService>();
            var entityService = ServiceLocator.Get<EntityService>();

            // Disconnect network
            await networkService.ShutdownAsync();

            // Unload data
            playerService.UnloadPlayerData();
            entityService.UnloadEntitiesData();

            gameState.RequestNewScene(GamePhase.Login);
        }
        #endregion

        #region Senders
        #endregion

        #region Receivers
        #endregion
        #endregion
    }
}
