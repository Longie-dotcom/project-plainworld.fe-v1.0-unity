using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.State
{
    public class EntityState
    {
        #region Attributes
        private readonly Dictionary<Guid, Vector2> positions = new();
        #endregion

        #region Properties
        public event Action<Guid, Vector2> OnEntityAdded;
        public event Action<Guid, Vector2> OnEntityMoved;
        public event Action<Guid> OnEntityRemoved;
        #endregion

        public EntityState() { }

        #region Methods
        public void AddEntity(Guid id, Vector2 position)
        {
            if (positions.ContainsKey(id)) return;

            positions[id] = position;
            OnEntityAdded?.Invoke(id, position);
        }

        public void UpdateEntityPosition(Guid id, Vector2 position)
        {
            if (!positions.ContainsKey(id)) return;

            positions[id] = position;
            OnEntityMoved?.Invoke(id, position);
        }

        public void RemoveEntity(Guid id)
        {
            if (!positions.Remove(id)) return;
            OnEntityRemoved?.Invoke(id);
        }
        #endregion
    }
}
