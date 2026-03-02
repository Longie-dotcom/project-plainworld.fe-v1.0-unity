using Assets.State.Component.Entity;
using Assets.State.Component.Player;
using Assets.State.Component.Shared;
using Assets.State.Interface.State;
using Assets.Utility;
using System;
using System.Collections.Generic;

namespace Assets.State
{
    public class EntityState : IReadOnlyEntityState
    {
        #region Attributes
        private readonly Dictionary<Guid, PlayerEntity> playerEntities = new();

        private readonly Dictionary<Guid, GrayShroomEntity> grayShroomEntities = new();

        #endregion

        #region Properties
        public event Action<PlayerEntity> OnPlayerEntityAdded;
        public event Action<Guid, PlayerEntity> OnPlayerEntityRemoved;

        public event Action<GrayShroomEntity> OnGrayShroomEntityAdded;
        public event Action<Guid, GrayShroomEntity> OnGrayShroomEntityRemoved;
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

        public void UpdatePlayerEntityAction(Guid id, ActSnapshot act)
        {
            if (!playerEntities.TryGetValue(id, out var playerEntity)) return;
            playerEntity.ApplyActionSnapshot(act);
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

        #region Gray Shroom Entity
        public IReadOnlyCollection<GrayShroomEntity> GetAllGrayShroomEntities()
        {
            return grayShroomEntities.Values;
        }

        public bool TryGetGrayShroom(Guid id, out GrayShroomEntity grayShroom)
        {
            return grayShroomEntities.TryGetValue(id, out grayShroom);
        }

        public void AddGrayShroomEntity(GrayShroomEntity grayShroomEntity)
        {
            if (grayShroomEntities.ContainsKey(grayShroomEntity.ID)) return;
            grayShroomEntities[grayShroomEntity.ID] = grayShroomEntity;

            // Note: First fired will be re-called later
            OnGrayShroomEntityAdded?.Invoke(grayShroomEntity);
        }

        public void UpdateGrayShroomEntityAction(Guid id, ActSnapshot act)
        {
            if (!grayShroomEntities.TryGetValue(id, out var grayShroomEntity)) return;
            grayShroomEntity.ApplyActionSnapshot(act);
        }

        public void RemoveGrayShroomEntity(Guid id)
        {
            if (!grayShroomEntities.TryGetValue(id, out var grayShroomEntity)) return;
            grayShroomEntities.Remove(id); OnGrayShroomEntityRemoved?.Invoke(id, grayShroomEntity);
        }
        #endregion
        #endregion
    }
}
