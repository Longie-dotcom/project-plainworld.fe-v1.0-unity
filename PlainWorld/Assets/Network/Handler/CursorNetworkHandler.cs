using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;

namespace Assets.Network.Handler
{
    public class CursorNetworkHandler :
        ICursorNetworkReceiver,
        ICursorNetworkCommand
    {
        #region Attributes
        private CursorService cursorService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public CursorNetworkHandler() { }

        #region Methods
        public void BindService(CursorService service, NetworkService network)
        {
            cursorService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        #endregion

        #region Receive Handlers
        #endregion
    }
}
