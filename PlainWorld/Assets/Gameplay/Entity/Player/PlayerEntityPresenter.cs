using Assets.Service;
using Assets.State.Component.Entity;
using Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent;
using System;
using UnityEngine;

namespace Assets.Gameplay.Entity.Player
{
    public class PlayerEntityPresenter
        : EntityPresenter<PlayerEntityView, PlayerEntity>
    {
        #region Attributes
        private readonly PlayerEntityView entityPlayerPrefab;

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
            PlayerEntityView prefab,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog,
            EntityPartCatalog skinCatalog)
            : base(entityService)
        {
            entityPlayerPrefab = prefab;

            this.hairCatalog = hairCatalog;
            this.glassesCatalog = glassesCatalog;
            this.shirtCatalog = shirtCatalog;
            this.pantCatalog = pantCatalog;
            this.shoeCatalog = shoeCatalog;
            this.eyesCatalog = eyesCatalog;
            this.skinCatalog = skinCatalog;

            entityService.EntityState.OnPlayerEntityAdded += SpawnEntity;
            entityService.EntityState.OnPlayerEntityRemoved += RemoveEntity;
        }

        #region Methods
        public override void Dispose()
        {
            if (disposed) return;

            entityService.EntityState.OnPlayerEntityAdded -= SpawnEntity;
            entityService.EntityState.OnPlayerEntityRemoved -= RemoveEntity;

            base.Dispose();
        }

        protected override void SpawnEntity(PlayerEntity entity)
        {
            if (entityViews.ContainsKey(entity.ID)) return;

            var view = GameObject.Instantiate(
                entityPlayerPrefab,
                entity.Movement.Position,
                Quaternion.identity);
            view.Initialize(
                entity.ID,
                entity.Movement.Position);

            entityViews[entity.ID] = view;
            BindView(view, entity);
        }

        protected override void RemoveEntity(Guid id, PlayerEntity entity)
        {
            if (entityViews.TryGetValue(id, out var view))
            {
                GameObject.Destroy(view.gameObject);
                UnbindView(view, entity);
                entityViews.Remove(id);
            }
        }

        protected override void BindView(PlayerEntityView view, PlayerEntity entity)
        {
            void ApplyAppearance()
            {
                ApplyAppearanceToView(entity.ID, entity.Appearance);
            };

            // Outbound
            entity.Appearance.OnChanged += ApplyAppearance; ApplyAppearance();
            entity.Movement.OnMoveSpeedChanged += view.SetAnimationSpeed;
            entity.Movement.OnPositionChanged += view.ApplyPosition;
            entity.Movement.OnDirectionChanged += view.SetDirection;
            entity.Movement.OnActionChanged += view.SetAction;
        }

        protected override void UnbindView(PlayerEntityView view, PlayerEntity entity)
        {
            void ApplyAppearance()
            {
                ApplyAppearanceToView(entity.ID, entity.Appearance);
            };

            // Outbound
            entity.Appearance.OnChanged -= ApplyAppearance;
            entity.Movement.OnMoveSpeedChanged -= view.SetAnimationSpeed;
            entity.Movement.OnPositionChanged -= view.ApplyPosition;
            entity.Movement.OnDirectionChanged -= view.SetDirection;
            entity.Movement.OnActionChanged -= view.SetAction;
        }

        private void ApplyAppearanceToView(Guid id, IReadOnlyPlayerAppearance appearance)
        {
            if (!TryGetView(id, out var view)) return;

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

        #region Private Helpers
        #endregion
    }
}
