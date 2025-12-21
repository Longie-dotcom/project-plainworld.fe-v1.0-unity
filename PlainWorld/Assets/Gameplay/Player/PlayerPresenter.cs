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
        
        private PlayerMoveView playerPrefab;
        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public PlayerPresenter(
            PlayerService playerService,
            AuthService authService,
            PlayerMoveView playerPrefab)
        {
            this.playerService = playerService;
            this.authService = authService;
            this.playerPrefab = playerPrefab;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            playerService.PlayerState.OnCreatePlayer -= OnCreatePlayer;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(playerPrefab));

            playerService.PlayerState.OnCreatePlayer += OnCreatePlayer;
        }

        private void OnMove(Vector2 dir)
        {
            AsyncHelper.Run(() => playerService.MoveAsync(dir));
        }

        private void OnCreatePlayer()
        {
            // Instantiate
            var instance = GameObject.Instantiate(playerPrefab);
            instance.name = $"MainPlayer_{authService.Claims.FullName}";

            var view = instance.GetComponent<PlayerMoveView>();

            // Bind view events
            view.OnMove += OnMove;
            playerService.PlayerState.OnPositionChanged += view.ApplyPosition;

            // Replace prefab reference with instance
            playerPrefab = view;
        }
        #endregion
    }
}