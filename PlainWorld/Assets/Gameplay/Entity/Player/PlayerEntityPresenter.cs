using Assets.Service;
using Assets.State.Entity.Player;
using Assets.State.Player;
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

        public override void SpawnEntity(PlayerEntity entity)
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

        public override void RemoveEntity(Guid id)
        {
            if (entityViews.TryGetValue(id, out var view))
            {
                if (entityService.EntityState.TryGetPlayer(id, out var entity))
                {
                    UnbindView(view, entity);
                    GameObject.Destroy(view.gameObject);
                    entityViews.Remove(id);
                }
            }
        }

        protected override void BindView(PlayerEntityView view, PlayerEntity entity)
        {
            entity.Movement.OnMoveSpeedChanged += view.SetAnimationSpeed;
            entity.Movement.OnPositionChanged += view.ApplyPosition;
            entity.Movement.OnDirectionChanged += view.SetDirection;
            entity.Movement.OnActionChanged += view.SetAction;

            // Subscribe
            entityService.EntityState.OnPlayerEntityAppearanceLoaded += ApplyAppearanceToView;
        }

        protected override void UnbindView(PlayerEntityView view, PlayerEntity entity)
        {
            // Execute cleanup
        }

        // Helper to immediately apply appearance
        private void ApplyAppearanceToView(Guid id, PlayerAppearance appearance)
        {
            if (!TryGetView(id, out var view)) return;

            view.ApplyAppearance(
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
