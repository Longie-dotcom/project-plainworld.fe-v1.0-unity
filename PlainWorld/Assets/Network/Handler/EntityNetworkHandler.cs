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
        #endregion

        #region Receive Handlers
        public void OnPlayerEntityJoined(PlayerEntityDTO dto)
        {
            entityService.OnPlayerEntityJoined(dto);
        }

        public void OnPlayerEntityLogout(Guid id)
        {
            entityService.OnPlayerEntityLogout(id);
        }

        public void OnPlayerEntityMoved(PlayerEntityMovementDTO dto)
        {
            entityService.OnPlayerEntityMoved(dto);
        }

        public void OnPlayerEntityCreatedAppearance(PlayerEntityAppearanceDTO dto)
        {
            entityService.OnPlayerEntityCreatedAppearance(dto);
        }
        #endregion
    }
}
