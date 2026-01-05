using Assets.Service;
using Assets.State.Component.Entity;
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
            playerEntity.Movement.OnPositionChanged += view.ApplyPosition;
            playerEntity.Movement.OnDirectionChanged += view.SetDirection;
            playerEntity.Movement.OnActionChanged += view.SetAction;
            settingService.SettingState.OnChanged += view.ApplySettings;
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
            view.ApplyAppearance(
                hairCatalog.GetPartFrame(appearance.HairID),
                glassesCatalog.GetPartFrame(appearance.GlassesID),
                shirtCatalog.GetPartFrame(appearance.ShirtID),
                pantCatalog.GetPartFrame(appearance.PantID),
                shoeCatalog.GetPartFrame(appearance.ShoeID),
                eyesCatalog.GetPartFrame(appearance.EyesID),
                skinCatalog.GetPartFrame(appearance.SkinID),

                appearance.HairColor,
                appearance.PantColor,
                appearance.EyeColor,
                appearance.SkinColor
            );
        }
        #endregion
    }
}
