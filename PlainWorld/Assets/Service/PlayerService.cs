using Assets.Core;
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
        public PlayerState State { get; private set; } = new PlayerState();
        #endregion

        public PlayerService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (PlayerNetworkCommand == null)
                throw new InvalidOperationException(
                    "PlayerNetworkCommand not bound before Initialize");

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
            await PlayerNetworkCommand.Join(playerId, playerName);
            State.MarkJoined(playerId, playerName);
        }

        public async Task MoveAsync(Vector2 dir)
        {
            if (!State.HasJoined) 
                return;

            var next = State.PredictMove(dir);
            await PlayerNetworkCommand.Move(State.PlayerID, next);
            State.ApplyPredictedPosition(next);
        }

        // Receivers
        public void HandleUpdatePosition(float x, float y)
        {
            CoroutineRunner.Instance.Schedule(() =>
                State.ApplyServerPosition(
                    new Vector2(x, y))
            );
        }
        #endregion
    }
}
