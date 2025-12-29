using Assets.Data.Enum;
using Assets.Service;
using Assets.State.Game;
using Assets.Utility;
using System;
using UnityEngine;

namespace Assets.Gameplay.Player
{
    public class PlayerPresenter : IDisposable
    {
        #region Attributes
        private readonly PlayerService playerService;
        private readonly GameService gameService;
        private PlayerView playerView;
        private readonly PlayerView playerViewPrefab;

        private readonly EntityPartCatalog hairCatalog;
        private readonly EntityPartCatalog glassesCatalog;
        private readonly EntityPartCatalog shirtCatalog;
        private readonly EntityPartCatalog pantCatalog;
        private readonly EntityPartCatalog shoeCatalog;
        private readonly EntityPartCatalog eyesCatalog;
        private readonly EntityPartCatalog skinCatalog;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public PlayerPresenter(
            PlayerService playerService,
            GameService gameService,
            PlayerView playerViewPrefab,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog,
            EntityPartCatalog skinCatalog)
        {
            this.playerService = playerService;
            this.gameService = gameService;
            this.playerViewPrefab = playerViewPrefab;

            this.hairCatalog = hairCatalog;
            this.glassesCatalog = glassesCatalog;
            this.shirtCatalog = shirtCatalog;
            this.pantCatalog = pantCatalog;
            this.shoeCatalog = shoeCatalog;
            this.eyesCatalog = eyesCatalog;
            this.skinCatalog = skinCatalog;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            if (playerView != null)
            {
                playerView.OnUpdateVisualMove -= OnUpdateVisualMove;
                playerView.OnSendMoveToServer -= OnSendMoveToServer;
            }

            // Outbound
            if (playerView != null)
            {
                playerService.PlayerState.Appearance.OnChanged -= ApplyAppearance;
                playerService.PlayerState.Movement.OnPositionChanged -= playerView.ApplyPosition;
                playerService.PlayerState.Movement.OnDirectionChanged -= playerView.SetDirection;
                playerService.PlayerState.Movement.OnActionChanged -= playerView.SetAction;
                playerService.PlayerState.Movement.OnMoveSpeedChanged -= playerView.SetAnimationSpeed;

            }
            playerService.PlayerState.OnPlayerNeedsCustomization -= OnPlayerNeedsCustomization;
            playerService.PlayerState.OnPlayerReady -= OnPlayerReady;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(PlayerPresenter));

            // Outbound
            playerService.PlayerState.OnPlayerNeedsCustomization += OnPlayerNeedsCustomization;
            playerService.PlayerState.OnPlayerReady += OnPlayerReady;
        }

        private void OnPlayerNeedsCustomization()
        {
            gameService.GameState.SetPhase(GamePhase.CustomizeCharacter);
        }

        private void OnPlayerReady()
        {
            if (playerView != null)
                return; // already spawned

            // Instantiate
            var instance = GameObject.Instantiate(playerViewPrefab);
            instance.name = $"MainPlayer_{playerService.PlayerState.PlayerName}";

            // Replace with new instance
            playerView = instance;

            // Bind view events
            // Inbound
            playerView.OnUpdateVisualMove += OnUpdateVisualMove;
            playerView.OnSendMoveToServer += OnSendMoveToServer;

            // Outbound
            playerService.PlayerState.Appearance.OnChanged += ApplyAppearance; ApplyAppearance();
            playerService.PlayerState.Movement.OnPositionChanged += playerView.ApplyPosition;
            playerService.PlayerState.Movement.OnDirectionChanged += playerView.SetDirection;
            playerService.PlayerState.Movement.OnActionChanged += playerView.SetAction;
            playerService.PlayerState.Movement.OnMoveSpeedChanged += playerView.SetAnimationSpeed;

            // Start the game
            gameService.GameState.SetPhase(GamePhase.InGame);
        }

        #region Movement
        private void OnUpdateVisualMove(Vector2 dir)
        {
            if (playerView == null) return;
            playerService.PlayerState.ApplyPredictedPosition(dir);
        }

        private void OnSendMoveToServer()
        {
            AsyncHelper.Run(() => playerService.MoveAsync());
        }
        #endregion

        #region Appearance
        private void ApplyAppearance()
        {
            if (playerView == null)
                return;

            var appearance = playerService.PlayerState.Appearance;

            playerView.ApplyAppearance(
                ResolveFrame(hairCatalog, appearance.HairID),
                ResolveFrame(glassesCatalog, appearance.GlassesID),
                ResolveFrame(shirtCatalog, appearance.ShirtID),
                ResolveFrame(pantCatalog, appearance.PantID),
                ResolveFrame(shoeCatalog, appearance.ShoeID),
                ResolveFrame(eyesCatalog, appearance.EyesID),
                ResolveFrame(skinCatalog, appearance.SkinID),

                appearance.HairColor,
                appearance.PantColor,
                appearance.EyeColor,
                appearance.SkinColor
            );
        }
        #endregion
        #endregion

        #region Private Helpers
        private EntityPartFrame ResolveFrame(EntityPartCatalog catalog, string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var descriptor = catalog.GetDescriptor(id);
            return descriptor != null ? descriptor : null;
        }
        #endregion
    }
}