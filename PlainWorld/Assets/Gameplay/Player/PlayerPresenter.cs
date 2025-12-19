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

        private PlayerMoveView playerMoveView;
        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public PlayerPresenter(PlayerService playerService)
        {
            this.playerService = playerService;
        }

        #region Methods
        public void Bind(PlayerMoveView playerView)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(PlayerMoveView));

            this.playerMoveView = playerView;

            playerView.OnMove += OnMove;

            playerService.State.OnPositionChanged += playerView.ApplyPosition;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            playerMoveView.OnMove -= OnMove;

            playerService.State.OnPositionChanged -= playerMoveView.ApplyPosition;
        }

        private void OnMove(Vector2 dir)
        {
            AsyncHelper.Run(() => playerService.MoveAsync(dir));
        }
        #endregion
    }
}