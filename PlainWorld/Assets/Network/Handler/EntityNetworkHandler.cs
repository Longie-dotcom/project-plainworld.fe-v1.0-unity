using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using Assets.Utility;
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
        #region Player Entity
        public void OnPlayerEntityJoined(PlayerEntityDTO dto)
        {
            entityService.OnPlayerEntityJoined(dto);
        }

        public void OnPlayerEntityLogout(Guid id)
        {
            entityService.OnPlayerEntityLogout(id);
        }

        public void OnPlayerEntityActed(PlayerEntityActDTO dto)
        {
            entityService.OnPlayerEntityActed(dto);
        }

        public void OnPlayerEntityCreatedAppearance(PlayerEntityAppearanceDTO dto)
        {
            entityService.OnPlayerEntityCreatedAppearance(dto);
        }
        #endregion

        #region Gray Shroom
        public void OnGrayShroomEntitySpawned(GrayShroomEntityDTO dto)
        {
            entityService.OnGrayShroomEntitySpawned(dto);
        }

        public void OnGrayShroomEntityActed(GrayShroomEntityActDTO dto)
        {
            entityService.OnGrayShroomEntityActed(dto);
        }

        public void OnGrayShroomEntityDespawned(Guid id)
        {
            entityService.OnGrayShroomEntityDespawned(id);
        }
        #endregion
        #endregion
    }
}
