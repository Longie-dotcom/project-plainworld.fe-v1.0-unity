using System;

namespace Assets.State.Interface.Component.Entity
{
    public interface IReadOnlyBaseEntity
    {
        Guid ID { get; }
    }
}
