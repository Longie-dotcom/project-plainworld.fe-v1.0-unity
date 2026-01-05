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
        protected readonly SettingService settingService;
        protected readonly Dictionary<Guid, TEntityView> entityViews = new();

        protected bool disposed = false;
        #endregion

        #region Properties
        #endregion

        protected EntityPresenter(EntityService entityService, SettingService settingService)
        {
            this.entityService = entityService;
            this.settingService = settingService;
        }

        #region Methods
        public void Initialize()
        {
            foreach (var entity in GetExistingEntities())
            {
                SpawnEntity(entity);
            }

            SubscribeEvents();
        }

        public virtual void Dispose()
        {
            disposed = true;
            UnsubscribeEvents();
            entityViews.Clear();
        }

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

        // Derived classes provide the source of existing entities
        protected abstract IEnumerable<TEntity> GetExistingEntities();

        // Derived classes handle runtime event subscriptions
        protected abstract void SubscribeEvents();
        protected abstract void UnsubscribeEvents();

        protected abstract void SpawnEntity(TEntity entity);
        protected abstract void RemoveEntity(Guid id, TEntity entity);

        protected abstract void BindView(TEntityView view, TEntity entity);
        protected abstract void UnbindView(TEntityView view, TEntity entity);
        #endregion
    }
}
