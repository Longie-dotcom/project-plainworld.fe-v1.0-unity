using Assets.State.Component.Player;
using Assets.State.Interface.IReadOnlyComponent.IReadOnlyEntityComponent;
using Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent;
using System;

namespace Assets.State.Component.Entity
{
    public class PlayerEntity : BaseEntity, IReadOnlyPlayerEntity
    {
        #region Attributes
        private readonly PlayerMovement movement;
        private readonly PlayerAppearance appearance;
        #endregion

        #region Properties
        public string Name { get; private set; }

        public IReadOnlyPlayerMovement Movement { get { return movement; } }
        public IReadOnlyPlayerAppearance Appearance { get { return appearance; } }
        #endregion

        public PlayerEntity(
            Guid id, 
            string name,
            PlayerMovementSnapshot movement,
            PlayerAppearanceSnapshot appearance) : base(id)
        {
            Name = name;
            this.movement = new PlayerMovement();
            this.appearance = new PlayerAppearance();
            this.movement.ApplySnapshot(movement);
            this.appearance.ApplySnapshot(appearance);
        }

        #region Methods
        #region Movement
        public void ApplyMovementSnapshot(PlayerMovementSnapshot snapshot)
        {
            movement.ApplySnapshot(snapshot);
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
