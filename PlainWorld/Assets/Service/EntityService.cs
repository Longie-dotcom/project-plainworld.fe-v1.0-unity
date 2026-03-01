using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Service.Interface;
using Assets.State;
using Assets.State.Component.Entity;
using Assets.State.Interface.State;
using Assets.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public void UnloadEntitiesData()
        {
            entityState.UnloadEntitiesData();
        }

        #region Player Entity
        public IReadOnlyCollection<PlayerEntity> GetAllPlayerEntities()
        {
            return entityState.GetAllPlayerEntities();
        }
        #endregion

        #region Gray Shroom Entity
        public IReadOnlyCollection<GrayShroomEntity> GetAllGrayShroomEntities()
        {
            return entityState.GetAllGrayShroomEntities();
        }
        #endregion

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
                        ActMapper.ToSnapshot(dto.Act),
                        PlayerAppearanceMapper.ToSnapshot(dto.Appearance)))
            );
        }

        public void OnPlayerEntityActed(PlayerEntityActDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                entityState.UpdatePlayerEntityAction(
                    dto.ID,
                    ActMapper.ToSnapshot(dto.Act))
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

        #region Gray Shroom Entity
        public void OnGrayShroomEntitySpawned(GrayShroomEntityDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                entityState.AddGrayShroomEntity(
                    new GrayShroomEntity(
                        dto.ID,
                        ActMapper.ToSnapshot(dto.Act)))
            );
        }

        public void OnGrayShroomEntityActed(GrayShroomEntityActDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                entityState.UpdateGrayShroomEntityAction(
                    dto.ID,
                    ActMapper.ToSnapshot(dto.Act))
            );
        }

        public void OnGrayShroomEntityDespawned(Guid id)
        {
            CoroutineRunner.Instance.Schedule(() =>
                entityState.RemoveGrayShroomEntity(
                    id)
            );
        }
        #endregion
        #endregion
        #endregion
    }
}
