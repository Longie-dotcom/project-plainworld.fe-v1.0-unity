using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.State;
using System;
using System.Threading.Tasks;
using UnityEngine;

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
        public void OnPlayerEntityJoined(PlayerEntityDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                EntityState.AddEntity(
                    dto.ID,
                    new Vector2(dto.Position.X, dto.Position.Y))
            );
        }

        public void OnPlayerEntityMoved(PlayerEntityPositionDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                EntityState.UpdateEntityPosition(
                    dto.ID, 
                    new Vector2(dto.Position.X, dto.Position.Y))
            );
        }

        public void OnEntityLeft(Guid id)
        {
            CoroutineRunner.Instance.Schedule(() =>
                EntityState.RemoveEntity(
                    id)
            );
        }
        #endregion
    }
}
