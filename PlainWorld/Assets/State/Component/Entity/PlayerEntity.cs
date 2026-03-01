using Assets.State.Component.Player;
using Assets.State.Component.Shared;
using Assets.State.Interface.Component.Entity;
using Assets.State.Interface.Component.Player;
using Assets.State.Interface.Component.Shared;
using System;

namespace Assets.State.Component.Entity
{
    public class PlayerEntity : BaseEntity, IReadOnlyPlayerEntity
    {
        #region Attributes
        private readonly Act act;
        private readonly PlayerAppearance appearance;
        #endregion

        #region Properties
        public string Name { get; private set; }

        public IReadOnlyAct Act { get { return act; } }
        public IReadOnlyPlayerAppearance Appearance { get { return appearance; } }
        #endregion

        public PlayerEntity(
            Guid id,
            string name,
            ActSnapshot act,
            PlayerAppearanceSnapshot appearance) : base(id)
        {
            Name = name;
            this.act = new Act();
            this.appearance = new PlayerAppearance();
            this.act.ApplySnapshot(act);
            this.appearance.ApplySnapshot(appearance);
        }

        #region Methods
        #region Action
        public void ApplyActionSnapshot(ActSnapshot snapshot)
        {
            act.ApplySnapshot(snapshot);
        }
        #endregion

        #region Appearance
        public void ApplyAppearanceSnapshot(PlayerAppearanceSnapshot snapshot)
        {
            appearance.ApplySnapshot(snapshot);
        }
        #endregion
        #endregion
    }
}
