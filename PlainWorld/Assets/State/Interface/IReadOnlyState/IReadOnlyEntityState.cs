using Assets.State.Component.Entity;
using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyEntityState
    {
        event Action<PlayerEntity> OnPlayerEntityAdded;
        event Action<Guid, PlayerEntity> OnPlayerEntityRemoved;
    }
}
