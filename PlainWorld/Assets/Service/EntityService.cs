using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.State.Entity;
using Assets.State.Entity.Player;
using Assets.Utility;
using System;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class EntityService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IEntityNetworkCommand EntityNetworkCommand { get; private set; }
        public EntityState EntityState { get; } = new EntityState();
        #endregion

        public EntityService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (EntityNetworkCommand == null)
                throw new InvalidOperationException(
                    "EntityNetworkCommand not bound before Initialize");

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IEntityNetworkCommand command)
        {
            EntityNetworkCommand = command;
        }

        // Senders

        // Receivers
        #region Player Entity
        public void OnPlayerEntityJoined(PlayerEntityDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                EntityState.AddPlayerEntity(
                    new PlayerEntity(
                        dto.ID,
                        dto.FullName,
                        PlayerMovementMapper.ToSnapshot(dto.Movement),
                        PlayerAppearanceMapper.ToSnapshot(dto.Appearance)))
            );
        }

        public void OnPlayerEntityMoved(PlayerEntityMovementDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                EntityState.UpdatePlayerEntityPosition(
                    dto.ID,
                    PlayerMovementMapper.ToSnapshot(dto.Movement))
            );
        }

        public void OnPlayerEntityCreatedAppearance(PlayerEntityAppearanceDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                GameLogger.Info(
                    Channel.Service,
                    $"CREATED!!!!: with the SHIRT ID: {dto.Appearance.ShirtID}")
            );
        }
        #endregion

        public void OnPlayerEntityLogout(Guid id)
        {
            CoroutineRunner.Instance.Schedule(() =>
                EntityState.RemovePlayerEntity(
                    id)
            );
        }
        #endregion
    }
}
