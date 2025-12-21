using Assets.Service;
using System;
using UnityEngine;

namespace Assets.Gameplay.Entity.EntityPlayer
{
    public class EntityPlayerPresenter
        : EntityPresenter<EntityPlayerView>
    {
        #region Attributes
        private EntityPlayerView entityPlayerPrefab;
        #endregion

        #region Properties
        #endregion

        public EntityPlayerPresenter(
            EntityService entityService,
            EntityPlayerView entityPlayerPrefab
        ) : base(entityService)
        {
            this.entityPlayerPrefab = entityPlayerPrefab;

            entityService.EntityState.OnEntityAdded += SpawnEntity;
            entityService.EntityState.OnEntityMoved += UpdateEntityPosition;
            entityService.EntityState.OnEntityRemoved += RemoveEntity;
        }

        #region Methods
        public override void Dispose()
        {
            if (disposed) return;
            disposed = true;

            var state = entityService.EntityState;
            state.OnEntityAdded -= SpawnEntity;
            state.OnEntityMoved -= UpdateEntityPosition;
            state.OnEntityRemoved -= RemoveEntity;

            base.Dispose();
        }

        public override void SpawnEntity(Guid id, Vector2 position)
        {
            if (entityViews.ContainsKey(id)) return;

            var view = GameObject.Instantiate(entityPlayerPrefab, position, Quaternion.identity);
            view.Initialize(id, position);

            entityViews[id] = view;
            BindView(view);
        }

        public override void UpdateEntityPosition(Guid id, Vector2 position)
        {
            if (entityViews.TryGetValue(id, out var view))
                view.UpdatePosition(position);
        }

        protected override void BindView(EntityPlayerView view)
        {
            // future: view events go here
        }

        protected override void UnbindView(EntityPlayerView view)
        {
            // future: unsubscribe here
        }
        #endregion
    }
}
