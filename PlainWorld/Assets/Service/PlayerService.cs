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

        public PlayerState State { get; private set; }
        #endregion

        public PlayerService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (PlayerNetworkCommand == null)
                throw new InvalidOperationException(
                    "PlayerNetworkCommand not bound before Initialize");

            State = new PlayerState();

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

        public async Task JoinAsync()
        {
            await PlayerNetworkCommand.Join(State.PlayerId, State.PlayerName);
            State.MarkJoined();
        }

        public async Task MoveAsync(Vector2 dir)
        {
            var next = State.PredictMove(dir);
            await PlayerNetworkCommand.Move(State.PlayerId, next);
            State.ApplyPredictedPosition(next); // client-side prediction
        }

        // Called by network layer
        public void HandleUpdatePosition(float x, float y)
        {
            State.ApplyServerPosition(new Vector2(x, y));
        }
        #endregion
    }
}
