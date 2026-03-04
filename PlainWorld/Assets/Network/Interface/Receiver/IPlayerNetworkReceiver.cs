using Assets.Network.DTO;
using Assets.Network.Interface.Base;
using System;

namespace Assets.Network.Interface.Receiver
{
    public interface IPlayerNetworkReceiver : INetworkBase
    {
        void OnPlayerJoined(PlayerDTO dto);
        void OnPlayerLogout(Guid id);
        void OnPlayerActed(PlayerActDTO dto);
        void OnPlayerCreatedAppearance(PlayerAppearanceDTO dto);
        void OnPlayerForcedLogout();
        void OnPlayerPickedItem(Item item);
        void OnWorldObjectPlaced (WorldObjectDTO dto);
    }
}
