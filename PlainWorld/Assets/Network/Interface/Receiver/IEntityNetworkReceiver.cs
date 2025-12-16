using Assets.Network.DTO;
using Assets.Network.Interface.Base;

namespace Assets.Network.Interface.Receiver
{
    public interface IEntityNetworkReceiver : INetworkBase
    {
        void OnPlayerEntityJoined(PlayerJoinDTO dto);
        void OnPlayerEntityMoved(PlayerMoveDTO dto);
    }
}
