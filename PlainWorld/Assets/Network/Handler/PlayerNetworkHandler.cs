using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using System;
using System.Threading.Tasks;
using UnityEngine;

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
        public Task Join(Guid playerId, string playerName)
        {
            return sender.Send(
                OnSend.PlayerJoin,
                playerId,
                playerName
            );
        }

        public Task Move(Guid playerId, Vector2 position)
        {
            return sender.Send(
                OnSend.PlayerMove,
                playerId,
                position.x,
                position.y
            );
        }
        #endregion

        #region Receive Handlers
        public void OnPlayerJoined(PlayerJoinDTO dto)
        {
            if (dto.PlayerId != playerService.State.PlayerId) return;
            playerService.HandleUpdatePosition(dto.Position.X, dto.Position.Y);
        }

        public void OnPlayerMoved(PlayerMoveDTO dto)
        {
            if (dto.PlayerId != playerService.State.PlayerId) return;
            playerService.HandleUpdatePosition(dto.Position.X, dto.Position.Y);
        }
        #endregion
    }
}
