using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;

namespace Assets.Network.Handler
{
    public class ConsoleNetworkHandler :
        IConsoleNetworkReceiver,
        IConsoleNetworkCommand
    {
        #region Attributes
        private ConsoleService consoleService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public ConsoleNetworkHandler() { }

        #region Methods
        public void BindService(ConsoleService service, NetworkService network)
        {
            consoleService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        #endregion

        #region Receive Handlers
        #endregion
    }
}
