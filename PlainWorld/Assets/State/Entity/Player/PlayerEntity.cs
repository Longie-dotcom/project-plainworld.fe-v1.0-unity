using Assets.State.Player;
using System;
using UnityEngine;

namespace Assets.State.Entity.Player
{
    public class PlayerEntity : BaseEntity
    {
        #region Attributes
        #endregion

        #region Properties
        public string Name { get; private set; }

        public PlayerMovement Movement { get; }
        public PlayerAppearance Appearance { get; }
        #endregion

        public PlayerEntity(
            Guid id, 
            string name,
            PlayerMovementSnapshot movement,
            PlayerAppearanceSnapshot appearance) : base(id)
        {
            Name = name;
            Movement = new PlayerMovement();
            Appearance = new PlayerAppearance();
            Movement.ApplySnapshot(movement);
            Appearance.LoadFromSnapshot(appearance);
        }

        #region Methods
        #endregion
    }
}
