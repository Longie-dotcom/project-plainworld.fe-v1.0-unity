using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;

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
        public void OnPlayerEntityJoined(PlayerJoinDTO dto)
        {
            entityService.HandleRemotePlayerJoined(dto);
        }

        public void OnPlayerEntityMoved(PlayerMoveDTO dto)
        {
            entityService.HandleRemotePlayerMoved(dto);
        }
        #endregion

        #region Receive Handlers
        #endregion
    }
}
