using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;

namespace Assets.Network.Handler
{
    public class UINetworkHandler :
        IUINetworkReceiver,
        IUINetworkCommand
    {
        #region Attributes
        private UIService uiService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public UINetworkHandler() { }

        #region Methods
        public void BindService(UIService service, NetworkService network)
        {
            uiService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        #endregion

        #region Receive Handlers
        #endregion
    }
}
