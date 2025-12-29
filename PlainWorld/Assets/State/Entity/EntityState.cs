using Assets.State.Entity.Player;
using Assets.State.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.State.Entity
{
    public class EntityState
    {
        #region Attributes
        private readonly Dictionary<Guid, PlayerEntity> playerEntities = new();
        #endregion

        #region Properties
        public event Action<PlayerEntity> OnPlayerEntityAdded;
        public event Action<Guid> OnPlayerEntityRemoved;
        public event Action<Guid, PlayerAppearance> OnPlayerEntityAppearanceLoaded;
        #endregion

        public EntityState() { }

        #region Methods
        #region Player Entity
        public bool TryGetPlayer(Guid id, out PlayerEntity player)
        {
            return playerEntities.TryGetValue(id, out player);
        }

        public void AddPlayerEntity(PlayerEntity player)
        {
            if (playerEntities.ContainsKey(player.ID)) return;
            playerEntities[player.ID] = player;
            OnPlayerEntityAdded?.Invoke(player);
            OnPlayerEntityAppearanceLoaded?.Invoke(player.ID, player.Appearance);
        }

        public void UpdatePlayerEntityPosition(Guid id, PlayerMovementSnapshot movement)
        {
            if (!playerEntities.TryGetValue(id, out var player)) return;
            player.Movement.ApplySnapshot(movement);
        }

        public void RemovePlayerEntity(Guid id)
        {
            if (!playerEntities.Remove(id)) return;
            OnPlayerEntityRemoved?.Invoke(id);
        }
        #endregion
        #endregion
    }
}
