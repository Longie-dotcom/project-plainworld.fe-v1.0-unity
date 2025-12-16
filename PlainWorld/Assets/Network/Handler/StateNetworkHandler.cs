using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;

namespace Assets.Network.Handler
{
    public class StateNetworkHandler :
        IStateNetworkReceiver,
        IStateNetworkCommand
    {
        #region Attributes
        private StateService stateService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public StateNetworkHandler() { }

        #region Methods
        public void BindService(StateService service, NetworkService network)
        {
            stateService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        #endregion

        #region Receive Handlers
        #endregion
    }
}
