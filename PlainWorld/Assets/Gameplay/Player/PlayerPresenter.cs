using Assets.Network;
using Assets.Service;
using Assets.Service.Enum;
using Assets.State.Component.Player;
using Assets.UI.Enum;
using Assets.Utility;
using System;
using UnityEngine;

namespace Assets.Gameplay.Player
{
    public class PlayerPresenter : IDisposable
    {
        #region Attributes
        private readonly NetworkService networkService;
        private readonly PlayerService playerService;
        private readonly GameService gameService;
        private readonly UIService uiService;
        private PlayerView playerView;
        private readonly PlayerView playerViewPrefab;

        private readonly EntityPartCatalog hairCatalog;
        private readonly EntityPartCatalog glassesCatalog;
        private readonly EntityPartCatalog shirtCatalog;
        private readonly EntityPartCatalog pantCatalog;
        private readonly EntityPartCatalog shoeCatalog;
        private readonly EntityPartCatalog eyesCatalog;
        private readonly EntityPartCatalog skinCatalog;

        private bool isActive;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public PlayerPresenter(
            NetworkService networkService,
            PlayerService playerService,
            GameService gameService,
            UIService uiService,
            PlayerView playerViewPrefab,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog,
            EntityPartCatalog skinCatalog)
        {
            this.networkService = networkService;
            this.playerService = playerService;
            this.gameService = gameService;
            this.uiService = uiService;
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

            UnbindPlayer();

            // Outbound
            playerService.PlayerState.OnPlayerReady -= OnPlayerReady;
            playerService.PlayerState.OnPlayerLogout -= OnPlayerLogout;
            playerService.PlayerState.OnPlayerForcedLogout -= OnPlayerForcedLogout;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(PlayerPresenter));

            // Outbound
            playerService.PlayerState.OnPlayerReady += OnPlayerReady;
            playerService.PlayerState.OnPlayerLogout += OnPlayerLogout;
            playerService.PlayerState.OnPlayerForcedLogout += OnPlayerForcedLogout;
        }

        #region Movement
        private void OnUpdateVisualMove(Vector2 dir)
        {
            playerService.ApplyPredictedPosition(dir);
        }

        private void OnSendMoveToServer()
        {
            if (!isActive) return;
            AsyncHelper.Run(() => playerService.MoveAsync());
        }
        #endregion

        #region Appearance
        private void ApplyAppearance()
        {
            if (playerView == null)
                return;

            var appearance = playerService.PlayerState.Appearance;

            var snapshot = new PlayerAppearanceSnapshot(
                appearance.IsCreated,
                appearance.HairID,
                appearance.GlassesID,
                appearance.ShirtID,
                appearance.PantID,
                appearance.ShoeID,
                appearance.EyesID,
                appearance.SkinID,
                appearance.HairColor,
                appearance.PantColor,
                appearance.EyeColor,
                appearance.SkinColor
            );

            var defaults = new PlayerAppearanceSnapshot(
                false,
                hairCatalog.GetDescriptors()[0].ID,
                glassesCatalog.GetDescriptors()[0].ID,
                shirtCatalog.GetDescriptors()[0].ID,
                pantCatalog.GetDescriptors()[0].ID,
                shoeCatalog.GetDescriptors()[0].ID,
                eyesCatalog.GetDescriptors()[0].ID,
                skinCatalog.GetDescriptors()[0].ID,
                Color.white,
                Color.white,
                Color.white,
                Color.white
            );

            playerService.ApplyDefaultAppearance(snapshot, defaults);

            var a = playerService.PlayerState.Appearance;

            playerView.ApplyAppearance(
                hairCatalog.GetPartFrame(a.HairID),
                glassesCatalog.GetPartFrame(a.GlassesID),
                shirtCatalog.GetPartFrame(a.ShirtID),
                pantCatalog.GetPartFrame(a.PantID),
                shoeCatalog.GetPartFrame(a.ShoeID),
                eyesCatalog.GetPartFrame(a.EyesID),
                skinCatalog.GetPartFrame(a.SkinID),
                a.HairColor,
                a.PantColor,
                a.EyeColor,
                a.SkinColor
            );
        }
        #endregion

        #region Outbound
        private void OnPlayerReady()
        {
            isActive = true;

            if (playerView != null)
                return; // already spawned

            // Instantiate
            var instance = GameObject.Instantiate(playerViewPrefab);
            instance.name = $"MainPlayer_{playerService.PlayerState.PlayerName}";

            // Replace with new instance
            playerView = instance;

            // Bind view events
            BindPlayer();

            // Start the game
            gameService.SetPhase(GamePhase.InGame);
        }

        private void OnPlayerLogout()
        {
            AsyncHelper.Run(async () =>
            {
                isActive = false;

                if (playerView == null)
                    return; // nothing to clean up

                // Destroy view instance
                GameObject.Destroy(playerView.gameObject);

                // Unbind view events
                UnbindPlayer();

                // Back to the login and show pop up
                gameService.SetPhase(GamePhase.Login);
                uiService.ShowPopUp(
                    PopUpType.Information,
                    "Player logout successfully");

                // Disconnect network
                await networkService.ShutdownAsync();
            });
        }

        private void OnPlayerForcedLogout()
        {
            AsyncHelper.Run(async () =>
            {
                isActive = false;

                if (playerView == null)
                    return; // nothing to clean up

                // Destroy view instance
                GameObject.Destroy(playerView.gameObject);

                // Unbind view events
                UnbindPlayer();

                // Back to the login and show pop up
                gameService.SetPhase(GamePhase.Login);
                uiService.ShowPopUp(
                    PopUpType.Information,
                    "Player logout successfully");

                // Disconnect network
                await networkService.ShutdownAsync();
            });
        }
        #endregion
        #endregion

        #region Private Helpers
        private void UnbindPlayer()
        {
            if (playerView == null) return;

            // Inbound
            playerView.OnUpdateVisualMove -= OnUpdateVisualMove;
            playerView.OnSendMoveToServer -= OnSendMoveToServer;

            // Outbound
            playerService.PlayerState.Appearance.OnChanged -= ApplyAppearance;
            playerService.PlayerState.Movement.OnPositionChanged -= playerView.ApplyPosition;
            playerService.PlayerState.Movement.OnDirectionChanged -= playerView.SetDirection;
            playerService.PlayerState.Movement.OnActionChanged -= playerView.SetAction;
            playerService.PlayerState.Movement.OnMoveSpeedChanged -= playerView.SetAnimationSpeed;

            playerView = null;
        }

        private void BindPlayer()
        {
            if (playerView == null) return;

            // Inbound
            playerView.OnUpdateVisualMove += OnUpdateVisualMove;
            playerView.OnSendMoveToServer += OnSendMoveToServer;

            // Outbound
            playerService.PlayerState.Appearance.OnChanged += ApplyAppearance;
            ApplyAppearance();
            playerService.PlayerState.Movement.OnMoveSpeedChanged += playerView.SetAnimationSpeed; 
            playerView.SetAnimationSpeed(playerService.PlayerState.Movement.MoveSpeed);
            playerService.PlayerState.Movement.OnPositionChanged += playerView.ApplyPosition;
            playerView.ApplyPosition(playerService.PlayerState.Movement.Position);
            playerService.PlayerState.Movement.OnDirectionChanged += playerView.SetDirection;
            playerView.SetDirection(playerService.PlayerState.Movement.CurrentDirection);
            playerService.PlayerState.Movement.OnActionChanged += playerView.SetAction;
            playerView.SetAction(playerService.PlayerState.Movement.CurrentAction);
        }
        #endregion
    }
}