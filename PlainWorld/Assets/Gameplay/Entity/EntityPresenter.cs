using Assets.Service;
using System;
using System.Collections.Generic;

namespace Assets.Gameplay.Entity
{
    public abstract class EntityPresenter<TEntityView, TEntity>
        where TEntityView : EntityView
    {
        #region Attributes
        protected readonly EntityService entityService;
        protected readonly Dictionary<Guid, TEntityView> entityViews = new();

        protected bool disposed = false;
        #endregion

        #region Properties
        #endregion

        public EntityPresenter(EntityService entityService)
        {
            this.entityService = entityService;
        }

        #region Methods
        public virtual void Dispose()
        {
            disposed = true;
            entityViews.Clear();
        }

        public abstract void SpawnEntity(TEntity entity);
        public abstract void RemoveEntity(Guid id);

        public bool TryGetView(Guid id, out TEntityView view)
        {
            return entityViews.TryGetValue(id, out view);
        }

        public TEntityView GetView(Guid id)
        {
            if (!entityViews.TryGetValue(id, out var view))
                throw new KeyNotFoundException($"Entity view not found for ID: {id}");
            return view;
        }

        protected abstract void BindView(TEntityView view, TEntity entity);
        protected abstract void UnbindView(TEntityView view, TEntity entity);
        #endregion
    }
}
