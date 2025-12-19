using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;

namespace Assets.Network.Handler
{
    public class GameNetworkHandler :
        IGameNetworkReceiver,
        IGameNetworkCommand
    {
        #region Attributes
        private GameService gameService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public GameNetworkHandler() { }

        #region Methods
        public void BindService(GameService service, NetworkService network)
        {
            gameService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        #endregion

        #region Receive Handlers
        #endregion
    }
}
