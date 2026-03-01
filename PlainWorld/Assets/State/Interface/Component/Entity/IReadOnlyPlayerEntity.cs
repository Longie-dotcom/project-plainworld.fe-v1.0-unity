using Assets.State.Interface.Component.Player;
using Assets.State.Interface.Component.Shared;

namespace Assets.State.Interface.Component.Entity
{
    public interface IReadOnlyPlayerEntity : IReadOnlyBaseEntity
    {
        string Name { get; }

        IReadOnlyAct Act { get; }
        IReadOnlyPlayerAppearance Appearance { get; }
    }
}
