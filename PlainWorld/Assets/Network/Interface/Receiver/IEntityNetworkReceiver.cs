using Assets.Network.DTO;
using Assets.Network.Interface.Base;
using System;

namespace Assets.Network.Interface.Receiver
{
    public interface IEntityNetworkReceiver : INetworkBase
    {
        void OnPlayerEntityJoined(PlayerEntityDTO dto);
        void OnPlayerEntityLogout(Guid id);
        void OnPlayerEntityActed(PlayerEntityActDTO dto);
        void OnPlayerEntityCreatedAppearance(PlayerEntityAppearanceDTO dto);

        void OnGrayShroomEntitySpawned(GrayShroomEntityDTO dto);
        void OnGrayShroomEntityActed(GrayShroomEntityActDTO dto);
        void OnGrayShroomEntityDespawned(Guid id);
    }
}
