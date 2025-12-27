using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.State;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Service
{
    public class PlayerService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IPlayerNetworkCommand PlayerNetworkCommand { get; private set; }
        public PlayerState PlayerState { get; private set; } = new PlayerState();
        #endregion

        public PlayerService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (PlayerNetworkCommand == null)
                throw new InvalidOperationException(
                    "PlayerNetworkCommand not bound before Initialize");

            ServiceLocator.OnExiting += LogoutAsync;

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IPlayerNetworkCommand command)
        {
            PlayerNetworkCommand = command;
        }

        // Senders
        public async Task JoinAsync(Guid playerId, string playerName)
        {
            var dto = new PlayerJoinDTO()
            {
                ID = playerId,
                Name = playerName
            };
            await PlayerNetworkCommand.Join(dto);

            PlayerState.MarkJoined(playerId, playerName);
        }

        public async Task LogoutAsync()
        {
            if (!PlayerState.HasJoined)
                return;

            await PlayerNetworkCommand.Logout(
                new PlayerLogoutDTO
                {
                    ID = PlayerState.PlayerID,
                    Name = PlayerState.PlayerName
                });
        }

        public async Task MoveAsync(Vector2 dir)
        {
            var next = PlayerState.PredictMove(dir);

            var dto = new PlayerMoveDTO()
            {
                ID = PlayerState.PlayerID,
                Position = new PositionDTO()
                {
                    X = next.x,
                    Y = next.y,
                },
            };
            await PlayerNetworkCommand.Move(dto);

            PlayerState.ApplyPredictedPosition(next);
        }

        // Receivers
        public void OnPlayerMoved(PlayerPositionDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                PlayerState.ApplyServerPosition(
                    dto.ID,
                    new Vector2(dto.Position.X, dto.Position.Y))
            );
        }

        public void OnPlayerJoined(PlayerDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                PlayerState.LoadPlayerData(
                    new Vector2(dto.Position.X, dto.Position.Y))
            );
        }
        #endregion
    }
}
