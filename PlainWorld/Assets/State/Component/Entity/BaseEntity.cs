using System;

namespace Assets.State.Component.Entity
{
    public class BaseEntity
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid ID { get; }
        #endregion

        protected BaseEntity(Guid id)
        {
            ID = id;
        }

        #region Methods
        #endregion
    }
}
