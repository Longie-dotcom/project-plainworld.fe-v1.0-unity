using Assets.Data.Enum;
using Assets.Service;
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
        private readonly PlayerService playerService;
        private readonly GameService gameService;
        private readonly UIService uiService;
        private readonly SettingService settingService;
        private PlayerView playerView;
        private readonly PlayerView playerViewPrefab;

        private readonly EntityPartCatalog hairCatalog;
        private readonly EntityPartCatalog glassesCatalog;
        private readonly EntityPartCatalog shirtCatalog;
        private readonly EntityPartCatalog pantCatalog;
        private readonly EntityPartCatalog shoeCatalog;
        private readonly EntityPartCatalog eyesCatalog;
        private readonly EntityPartCatalog skinCatalog;
        private readonly EntityPartCatalog itemCatalog;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public PlayerPresenter(
            PlayerService playerService,
            GameService gameService,
            UIService uiService,
            SettingService settingService,
            PlayerView playerViewPrefab,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog,
            EntityPartCatalog skinCatalog,
            EntityPartCatalog itemCatalog)
        {
            this.playerService = playerService;
            this.gameService = gameService;
            this.uiService = uiService;
            this.settingService = settingService;
            this.playerViewPrefab = playerViewPrefab;

            this.hairCatalog = hairCatalog;
            this.glassesCatalog = glassesCatalog;
            this.shirtCatalog = shirtCatalog;
            this.pantCatalog = pantCatalog;
            this.shoeCatalog = shoeCatalog;
            this.eyesCatalog = eyesCatalog;
            this.skinCatalog = skinCatalog;
            this.itemCatalog = itemCatalog;

            Bind();
            OnPlayerReady();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            UnbindPlayer();

            // Outbound
            playerService.PlayerState.OnPlayerLogout -= OnPlayerLogout;
            playerService.PlayerState.OnPlayerForcedLogout -= OnPlayerForcedLogout;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(PlayerPresenter));

            // Outbound
            playerService.PlayerState.OnPlayerLogout += OnPlayerLogout;
            playerService.PlayerState.OnPlayerForcedLogout += OnPlayerForcedLogout;
        }

        #region Action
        private void OnUpdateVisualAction(Vector2 dir, EntityAction entityAction)
        {
            playerService.ApplyPredictedAction(dir, entityAction);
        }

        private void OnSendActionToServer()
        {
            AsyncHelper.Run(() => playerService.ActAsync());
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
            if (playerView != null)
                return; // already spawned

            // Instantiate
            var instance = GameObject.Instantiate(playerViewPrefab);
            instance.name = $"MainPlayer_{playerService.PlayerState.PlayerName}";

            // Replace with new instance
            playerView = instance;

            // Bind view events
            BindPlayer();
        }

        private void OnPlayerLogout()
        {
            AsyncHelper.Run(async () =>
            {
                if (playerView == null)
                    return; // nothing to clean up

                // Unbind view events
                UnbindPlayer();

                // Destroy view instance
                GameObject.Destroy(playerView.gameObject);

                uiService.ShowPopUp(
                    PopUpType.Information,
                    "Player logout successfully");

                // Player logout is a player life-cycle phase
                await gameService.PlayerLogout();
            });
        }

        private void OnPlayerForcedLogout()
        {
            AsyncHelper.Run(async () =>
            {
                if (playerView == null)
                    return; // nothing to clean up

                // Unbind view events
                UnbindPlayer();

                // Destroy view instance
                GameObject.Destroy(playerView.gameObject);

                uiService.ShowPopUp(
                    PopUpType.Information,
                    "Player was forced to logout");

                // Player logout is a player life-cycle phase
                await gameService.PlayerLogout();
            });
        }
        #endregion
        #endregion

        #region Private Helpers
        private void UnbindPlayer()
        {
            if (playerView == null) return;

            // Inbound
            playerView.OnUpdateVisualAction -= OnUpdateVisualAction;
            playerView.OnSendActionToServer -= OnSendActionToServer;

            // Outbound
            playerService.PlayerState.Appearance.OnChanged -= ApplyAppearance;
            playerService.PlayerState.Act.OnMoveSpeedChanged -= playerView.SetSpeed;
            playerService.PlayerState.Act.OnPositionChanged -= playerView.ApplyPosition;
            playerService.PlayerState.Act.OnDirectionChanged -= playerView.SetDirection;
            playerService.PlayerState.Act.OnActionChanged -= playerView.SetAction;
            settingService.SettingState.OnChanged -= playerView.ApplySettings;
        }

        private void BindPlayer()
        {
            if (playerView == null) return;

            // Inbound
            playerView.OnUpdateVisualAction += OnUpdateVisualAction;
            playerView.OnSendActionToServer += OnSendActionToServer;

            // Outbound
            playerService.PlayerState.Appearance.OnChanged += ApplyAppearance;
            ApplyAppearance();

            playerService.PlayerState.Act.OnMoveSpeedChanged += playerView.SetSpeed;
            playerView.SetSpeed(playerService.PlayerState.Act.MoveSpeed);
            playerService.PlayerState.Act.OnPositionChanged += playerView.ApplyPosition;
            playerView.ApplyPosition(playerService.PlayerState.Act.Position);
            playerService.PlayerState.Act.OnDirectionChanged += playerView.SetDirection;
            playerView.SetDirection(playerService.PlayerState.Act.CurrentDirection);
            playerService.PlayerState.Act.OnActionChanged += playerView.SetAction;
            playerView.SetAction(playerService.PlayerState.Act.CurrentAction);
            playerService.PlayerState.Act.OnItemUsed += () => playerView.HoldItem(itemCatalog.GetDefault());  // TEST

            settingService.SettingState.OnChanged += playerView.ApplySettings;
            playerView.ApplySettings(settingService.SettingState);
        }
        #endregion
    }
}