using System;
using UnityEngine;

namespace Assets.State.Entity
{
    public class BaseEntity
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid ID { get; }
        #endregion

        public BaseEntity(Guid id)
        {
            ID = id;
        }

        #region Methods
        #endregion
    }
}
