using Assets.State.Component.Entity;
using System;

namespace Assets.State.Interface.State
{
    public interface IReadOnlyEntityState
    {
        event Action<PlayerEntity> OnPlayerEntityAdded;
        event Action<Guid, PlayerEntity> OnPlayerEntityRemoved;

        event Action<GrayShroomEntity> OnGrayShroomEntityAdded;
        event Action<Guid, GrayShroomEntity> OnGrayShroomEntityRemoved;
    }
}
