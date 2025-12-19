using Assets.Service;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Gameplay.Entity
{
    public abstract class EntityPresenter<TView> : IDisposable
        where TView : EntityView
    {
        #region Attributes
        protected readonly Dictionary<Guid, TView> entityViews = new();
        protected readonly EntityService entityService;

        protected bool disposed;
        #endregion

        #region Properties
        #endregion

        public EntityPresenter(EntityService entityService)
        {
            this.entityService = entityService;
        }

        #region Methods
        protected virtual void BindView(TView view)
        { 
        
        }

        protected virtual void UnbindView(TView view)
        { 
        
        }

        public virtual void Dispose()
        {
            foreach (var view in entityViews.Values)
                UnbindView(view);

            entityViews.Clear();
        }

        public virtual void RemoveEntity(Guid id)
        {
            if (!entityViews.TryGetValue(id, out var view)) return;

            UnbindView(view);
            GameObject.Destroy(view.gameObject);
            entityViews.Remove(id);
        }

        public abstract void SpawnEntity(Guid id, Vector2 position);
        public abstract void UpdateEntityPosition(Guid id, Vector2 position);
        #endregion
    }
}
