using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;

namespace Assets.Network.Handler
{
    public class SettingNetworkHandler :
        ISettingNetworkReceiver,
        ISettingNetworkCommand
    {
        #region Attributes
        private SettingService settingService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public SettingNetworkHandler() { }

        #region Methods
        public void BindService(SettingService service, NetworkService network)
        {
            settingService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        #endregion

        #region Receive Handlers
        #endregion
    }
}
