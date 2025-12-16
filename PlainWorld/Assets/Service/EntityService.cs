using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Service
{
    public class EntityService : IService
    {
        #region Attributes
        private readonly Dictionary<Guid, GameObject> remotePlayers = new();
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IEntityNetworkCommand EntityNetworkCommand { get; private set; }
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

        public void HandleRemotePlayerJoined(PlayerJoinDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                SpawnRemotePlayer(dto)
            );
        }

        public void HandleRemotePlayerMoved(PlayerMoveDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                UpdateRemotePlayer(dto)
            );
        }

        private void SpawnRemotePlayer(PlayerJoinDTO dto)
        {
            if (remotePlayers.ContainsKey(dto.PlayerId))
                return;

            var go = new GameObject($"RemotePlayer_{dto.PlayerId}");
            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = Color.red;
            go.transform.position = new Vector3(dto.Position.X, dto.Position.Y, 0);

            remotePlayers[dto.PlayerId] = go;
        }

        private void UpdateRemotePlayer(PlayerMoveDTO dto)
        {
            if (!remotePlayers.TryGetValue(dto.PlayerId, out var go))
                return;

            go.transform.position = new Vector3(dto.Position.X, dto.Position.Y, 0);
        }
        #endregion
    }
}
