using Assets.State.Component.Entity;
using Assets.State.Component.Player;
using Assets.State.Interface.IReadOnlyState;
using Assets.Utility;
using System;
using System.Collections.Generic;

namespace Assets.State
{
    public class EntityState : IReadOnlyEntityState
    {
        #region Attributes
        private readonly Dictionary<Guid, PlayerEntity> playerEntities = new();
        #endregion

        #region Properties
        public event Action<PlayerEntity> OnPlayerEntityAdded;
        public event Action<Guid, PlayerEntity> OnPlayerEntityRemoved;
        #endregion

        public EntityState() { }

        #region Methods
        public void UnloadEntitiesData()
        {
            foreach (var kvp in new Dictionary<Guid, PlayerEntity>(playerEntities))
            {
                RemovePlayerEntity(kvp.Key);
            }
        }

        #region Player Entity
        public IReadOnlyCollection<PlayerEntity> GetAllPlayerEntities()
        {
            return playerEntities.Values;
        }

        public bool TryGetPlayer(Guid id, out PlayerEntity player)
        {
            return playerEntities.TryGetValue(id, out player);
        }

        public void AddPlayerEntity(PlayerEntity playerEntity)
        {
            if (playerEntities.ContainsKey(playerEntity.ID)) return;
            playerEntities[playerEntity.ID] = playerEntity;

            // Note: First fired will be re-called later
            OnPlayerEntityAdded?.Invoke(playerEntity);
        }

        public void UpdatePlayerEntityPosition(Guid id, PlayerMovementSnapshot movement)
        {
            if (!playerEntities.TryGetValue(id, out var playerEntity)) return;
            playerEntity.ApplyMovementSnapshot(movement);
        }

        public void UpdatePlayerEntityAppearance(Guid id, PlayerAppearanceSnapshot appearance)
        {
            if (!playerEntities.TryGetValue(id, out var player)) return;
            player.ApplyAppearanceSnapshot(appearance);
        }

        public void RemovePlayerEntity(Guid id)
        {
            if (!playerEntities.TryGetValue(id, out var playerEntity)) return;
            playerEntities.Remove(id); OnPlayerEntityRemoved?.Invoke(id, playerEntity);
        }
        #endregion
        #endregion
    }
}
