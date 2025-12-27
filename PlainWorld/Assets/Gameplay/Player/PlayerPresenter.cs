using Assets.Data.Enum;
using Assets.Service;
using Assets.Utility;
using System;
using UnityEngine;

namespace Assets.Gameplay.Player
{
    public class PlayerPresenter : IDisposable
    {
        #region Attributes
        private readonly PlayerService playerService;
        private readonly AuthService authService;
        
        private PlayerView playerView;
        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public PlayerPresenter(
            PlayerService playerService,
            AuthService authService,
            PlayerView playerView)
        {
            this.playerService = playerService;
            this.authService = authService;
            this.playerView = playerView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            playerView.OnMove -= OnMove;
            playerView.OnStop -= OnStop;

            // Outbound
            playerService.PlayerState.OnCreatePlayer -= OnCreatePlayer;
            playerService.PlayerState.OnPositionChanged -= playerView.ApplyPosition;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(playerView));

            playerService.PlayerState.OnCreatePlayer += OnCreatePlayer;
        }

        private void OnCreatePlayer()
        {
            if (playerView != null)
            {
                playerView.OnMove -= OnMove;
                playerView.OnStop -= OnStop;
                playerService.PlayerState.OnPositionChanged -= playerView.ApplyPosition;
            }

            // Instantiate
            var instance = GameObject.Instantiate(playerView);
            instance.name = $"MainPlayer_{authService.Claims.FullName}";

            // Replace with new instance
            playerView = instance;

            // Bind view events
            // Inbound
            playerView.OnMove += OnMove;
            playerView.OnStop += OnStop;

            // Outbound
            playerService.PlayerState.OnPositionChanged += playerView.ApplyPosition;
        }

        private void OnMove(Vector2 dir)
        {
            // Visual decision (client-side, immediate)
            playerView.SetDirection(dir);
            playerView.SetAction(EntityAction.RUN);
            playerView.SetAnimationSpeed(playerService.PlayerState.MoveSpeed);

            // Networking
            AsyncHelper.Run(() => playerService.MoveAsync(dir));
        }

        private void OnStop()
        {
            playerView.SetAction(EntityAction.IDLE);
        }
        #endregion
    }
}