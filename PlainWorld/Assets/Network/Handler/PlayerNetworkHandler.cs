using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using Assets.Utility;
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
        public Task Join(PlayerJoinDTO dto)
        {
            return sender.Send(
                OnSend.PlayerJoin,
                dto
            );
        }

        public Task Logout(PlayerLogoutDTO dto)
        {
            return sender.Send(
                OnSend.PlayerLogout,
                dto
            );
        }

        public Task Move(PlayerMoveDTO dto)
        {
            return sender.Send(
                OnSend.PlayerMove,
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
            GameLogger.Info(
                Channel.Service, $"Player with ID: {id} has logout");
        }

        public void OnPlayerMoved(PlayerPositionDTO dto)
        {
            playerService.OnPlayerMoved(dto);
        }
        #endregion
    }
}
