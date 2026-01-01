using Assets.Core;
using Assets.Service.Interface;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.State;
using Assets.State.Component.Entity;
using Assets.Utility;
using System;
using System.Threading.Tasks;
using Assets.State.Interface.IReadOnlyState;

namespace Assets.Service
{
    public class EntityService : IService
    {
        #region Attributes
        private readonly EntityState entityState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IEntityNetworkCommand EntityNetworkCommand { get; private set; }
        public IReadOnlyEntityState EntityState { get { return entityState; } }
        #endregion

        public EntityService()
        {
            entityState = new EntityState();
        }

        #region Methods
        public Task InitializeAsync()
        {
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

        #region Senders
        #endregion

        #region Receivers
        #region Player Entity
        public void OnPlayerEntityJoined(PlayerEntityDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                entityState.AddPlayerEntity(
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
                entityState.UpdatePlayerEntityPosition(
                    dto.ID,
                    PlayerMovementMapper.ToSnapshot(dto.Movement))
            );
        }

        public void OnPlayerEntityCreatedAppearance(PlayerEntityAppearanceDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                entityState.UpdatePlayerEntityAppearance(
                    dto.ID,
                    PlayerAppearanceMapper.ToSnapshot(dto.Appearance))
            );
        }

        public void OnPlayerEntityLogout(Guid id)
        {
            CoroutineRunner.Instance.Schedule(() =>
                entityState.RemovePlayerEntity(
                    id)
            );
        }
        #endregion
        #endregion
        #endregion
    }
}
