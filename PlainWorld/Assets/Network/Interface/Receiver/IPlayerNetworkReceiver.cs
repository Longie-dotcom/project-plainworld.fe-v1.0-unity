using Assets.Network.DTO;
using Assets.Network.Interface.Base;

namespace Assets.Network.Interface.Receiver
{
    public interface IPlayerNetworkReceiver : INetworkBase
    {
        void OnPlayerJoined(PlayerJoinDTO dto);
        void OnPlayerMoved(PlayerMoveDTO dto);
    }
}
