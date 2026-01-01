using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using System;
using System.Threading.Tasks;

namespace Assets.Network.Handler
{
    public class PlayerNetworkHandler :
        IPlayerNetworkReceiver,
        IPlayerNetworkCommand
    {
        #region Attributes
        private PlayerService playerService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public PlayerNetworkHandler() { }

        #region Methods
        public void BindService(PlayerService service, NetworkService network)
        {
            playerService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        public Task Join()
        {
            return sender.Send(
                OnSend.PlayerJoin);
        }

        public Task Logout()
        {
            return sender.Send(
                OnSend.PlayerLogout);
        }

        public Task Move(PlayerMoveDTO dto)
        {
            return sender.Send(
                OnSend.PlayerMove,
                dto
            );
        }

        public Task CreateAppearance(PlayerCreateAppearanceDTO dto)
        {
            return sender.Send(
                OnSend.PlayerCreateAppearance,
                dto
            );
        }
        #endregion

        #region Receive Handlers
        public void OnPlayerJoined(PlayerDTO dto)
        {
            playerService.OnPlayerJoined(dto);
        }

        public void OnPlayerLogout(Guid id)
        {
            playerService.OnPlayerLogout(id);
        }

        public void OnPlayerMoved(PlayerMovementDTO dto)
        {
            playerService.OnPlayerMoved(dto);
        }

        public void OnPlayerCreatedAppearance(PlayerAppearanceDTO dto)
        {
            playerService.OnPlayerCreatedAppearance(dto);
        }

        public void OnPlayerForcedLogout()
        {
            playerService.OnPlayerForcedLogout();
        }
        #endregion
    }
}
