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
            gameState = new GameState(GamePhase.Login);
        }

        #region Methods
        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IGameNetworkCommand command)
        {
            GameNetworkCommand = command;
        }

        public void SetPhase(GamePhase gamePhase)
        {
            gameState.SetPhase(gamePhase);
        }

        public void PushPhase(GamePhase overlay)
        {
            gameState.PushPhase(overlay);
        }

        public void PopPhase()
        {
            gameState.PopPhase();
        }
        #region Senders
        #endregion

        #region Receivers
        #endregion
        #endregion
    }
}
