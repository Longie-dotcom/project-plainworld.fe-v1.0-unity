using Assets.State.Component.Player;
using Assets.State.Component.Shared;
using Assets.State.Interface.Component.Entity;
using Assets.State.Interface.Component.Shared;
using System;

namespace Assets.State.Component.Entity
{
    public class GrayShroomEntity : BaseEntity, IReadOnlyGrayShroomEntity
    {
        #region Attributes
        private readonly Act act;
        #endregion

        #region Properties
        public string Name { get; private set; }

        public IReadOnlyAct Act { get { return act; } }
        #endregion

        public GrayShroomEntity(
            Guid id,
            ActSnapshot act) : base(id)
        {
            this.act = new Act();
            this.act.ApplySnapshot(act);
        }

        #region Methods
        #region Action
        public void ApplyActionSnapshot(ActSnapshot snapshot)
        {
            act.ApplySnapshot(snapshot);
        }
        #endregion
        #endregion
    }
}
