using Assets.Service;
using Assets.State.Component.Entity;
using Assets.State.Component.Player;
using Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gameplay.Entity.Player
{
    public class PlayerEntityPresenter
        : EntityPresenter<PlayerEntityView, PlayerEntity>
    {
        #region Attributes
        private readonly PlayerEntityView playerEntityPrefab;

        private readonly EntityPartCatalog hairCatalog;
        private readonly EntityPartCatalog glassesCatalog;
        private readonly EntityPartCatalog shirtCatalog;
        private readonly EntityPartCatalog pantCatalog;
        private readonly EntityPartCatalog shoeCatalog;
        private readonly EntityPartCatalog eyesCatalog;
        private readonly EntityPartCatalog skinCatalog;
        #endregion

        #region Properties
        #endregion

        public PlayerEntityPresenter(
            EntityService entityService,
            SettingService settingService,
            PlayerEntityView prefab,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog,
            EntityPartCatalog skinCatalog)
            : base(entityService, settingService)
        {
            playerEntityPrefab = prefab;

            this.hairCatalog = hairCatalog;
            this.glassesCatalog = glassesCatalog;
            this.shirtCatalog = shirtCatalog;
            this.pantCatalog = pantCatalog;
            this.shoeCatalog = shoeCatalog;
            this.eyesCatalog = eyesCatalog;
            this.skinCatalog = skinCatalog;

            Initialize();
        }

        #region Methods
        protected override IEnumerable<PlayerEntity> GetExistingEntities()
        {
            return entityService.GetAllPlayerEntities();
        }

        protected override void SubscribeEvents()
        {
            entityService.EntityState.OnPlayerEntityAdded += SpawnEntity;
            entityService.EntityState.OnPlayerEntityRemoved += RemoveEntity;
        }

        protected override void UnsubscribeEvents()
        {
            entityService.EntityState.OnPlayerEntityAdded -= SpawnEntity;
            entityService.EntityState.OnPlayerEntityRemoved -= RemoveEntity;
        }

        protected override void SpawnEntity(PlayerEntity playerEntity)
        {
            // Make sure all entities are not duplicated
            if (entityViews.ContainsKey(playerEntity.ID)) return;

            // First fired does not catch up with the prefab so re-call later in the initialize
            if (playerEntityPrefab == null) return;

            var view = GameObject.Instantiate(
                playerEntityPrefab,
                playerEntity.Movement.Position,
                Quaternion.identity);
            view.Initialize(
                playerEntity.ID,
                playerEntity.Movement.Position);

            entityViews[playerEntity.ID] = view;
            BindView(view, playerEntity);
        }

        protected override void RemoveEntity(Guid id, PlayerEntity playerEntity)
        {
            if (entityViews.TryGetValue(id, out var view))
            {
                UnbindView(view, playerEntity);
                GameObject.Destroy(view.gameObject);
                entityViews.Remove(id);
            }
        }

        protected override void BindView(PlayerEntityView view, PlayerEntity playerEntity)
        {
            // Outbound
            playerEntity.Appearance.OnChanged += () => ApplyAppearanceToView(view, playerEntity.Appearance); 
            ApplyAppearanceToView(view, playerEntity.Appearance);
            playerEntity.Movement.OnMoveSpeedChanged += view.SetPlayerSpeed;
            view.SetPlayerSpeed(playerEntity.Movement.MoveSpeed);
            playerEntity.Movement.OnPositionChanged += view.ApplyPosition;
            view.ApplyPosition(playerEntity.Movement.Position);
            playerEntity.Movement.OnDirectionChanged += view.SetDirection;
            view.SetDirection(playerEntity.Movement.CurrentDirection);
            playerEntity.Movement.OnActionChanged += view.SetAction;
            view.SetAction(playerEntity.Movement.CurrentAction);
            settingService.SettingState.OnChanged += view.ApplySettings;
            view.ApplySettings(settingService.SettingState);
        }

        protected override void UnbindView(PlayerEntityView view, PlayerEntity playerEntity)
        {
            // Outbound
            playerEntity.Appearance.OnChanged -= () => ApplyAppearanceToView(view, playerEntity.Appearance);
            playerEntity.Movement.OnMoveSpeedChanged -= view.SetPlayerSpeed;
            playerEntity.Movement.OnPositionChanged -= view.ApplyPosition;
            playerEntity.Movement.OnDirectionChanged -= view.SetDirection;
            playerEntity.Movement.OnActionChanged -= view.SetAction;
            settingService.SettingState.OnChanged -= view.ApplySettings;
        }
        #endregion

        #region Private Helpers
        private void ApplyAppearanceToView(
            PlayerEntityView view,
            IReadOnlyPlayerAppearance appearance)
        {
            if (view == null)
                return;

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

            view.ApplyAppearance(
                hairCatalog.GetPartFrame(appearance.HairID ?? defaults.HairID),
                glassesCatalog.GetPartFrame(appearance.GlassesID ?? defaults.GlassesID),
                shirtCatalog.GetPartFrame(appearance.ShirtID ?? defaults.ShirtID),
                pantCatalog.GetPartFrame(appearance.PantID ?? defaults.PantID),
                shoeCatalog.GetPartFrame(appearance.ShoeID ?? defaults.ShoeID),
                eyesCatalog.GetPartFrame(appearance.EyesID ?? defaults.EyesID),
                skinCatalog.GetPartFrame(appearance.SkinID ?? defaults.SkinID),

                appearance.HairColor == default ? defaults.HairColor : appearance.HairColor,
                appearance.PantColor == default ? defaults.PantColor : appearance.PantColor,
                appearance.EyeColor == default ? defaults.EyeColor : appearance.EyeColor,
                appearance.SkinColor == default ? defaults.SkinColor : appearance.SkinColor
            );
        }
        #endregion
    }
}
