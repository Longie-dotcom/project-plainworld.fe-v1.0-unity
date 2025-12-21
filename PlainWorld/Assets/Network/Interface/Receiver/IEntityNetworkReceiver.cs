using Assets.Network.DTO;
using Assets.Network.Interface.Base;
using System;

namespace Assets.Network.Interface.Receiver
{
    public interface IEntityNetworkReceiver : INetworkBase
    {
        void OnPlayerEntityJoined(PlayerEntityDTO dto);
        void OnPlayerEntityMoved(PlayerEntityPositionDTO dto);
        void OnEntityLeft(Guid id);
    }
}
