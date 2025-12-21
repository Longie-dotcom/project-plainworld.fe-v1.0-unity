using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using System;

namespace Assets.Network.Handler
{
    public class EntityNetworkHandler :
        IEntityNetworkReceiver,
        IEntityNetworkCommand
    {
        #region Attributes
        private EntityService entityService;
        private NetworkCommandSender sender = new();
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public EntityNetworkHandler() { }

        #region Methods
        public void BindService(EntityService service, NetworkService network)
        {
            entityService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        public void OnPlayerEntityJoined(PlayerEntityDTO dto)
        {
            entityService.OnPlayerEntityJoined(dto);
        }

        public void OnPlayerEntityMoved(PlayerEntityPositionDTO dto)
        {
            entityService.OnPlayerEntityMoved(dto);
        }

        public void OnEntityLeft(Guid id)
        {
            entityService.OnEntityLeft(id);
        }
        #endregion

        #region Receive Handlers
        #endregion
    }
}
