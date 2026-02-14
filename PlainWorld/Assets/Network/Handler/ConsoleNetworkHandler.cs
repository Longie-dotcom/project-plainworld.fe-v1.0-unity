using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using System.Threading.Tasks;

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
        public Task Chat(ChatSendDTO dto)
        {
            return sender.Send(
                OnSend.PlayerChat,
                dto
            );
        }
        #endregion

        #region Receive Handlers
        public void OnPlayerChatted(ChatDTO dto)
        {
            consoleService.OnPlayerChatted(dto);
        }

        public void OnPlayerEntityChatted(ChatDTO dto)
        {
            consoleService.OnPlayerEntityChatted(dto);
        }
        #endregion
    }
}
