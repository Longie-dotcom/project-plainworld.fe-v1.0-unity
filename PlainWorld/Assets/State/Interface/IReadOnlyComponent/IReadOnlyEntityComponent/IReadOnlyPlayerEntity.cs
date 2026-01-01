using Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent;

namespace Assets.State.Interface.IReadOnlyComponent.IReadOnlyEntityComponent
{
    public interface IReadOnlyPlayerEntity : IReadOnlyBaseEntity
    {
        string Name { get; }

        IReadOnlyPlayerMovement Movement { get; }
        IReadOnlyPlayerAppearance Appearance { get; }
    }
}
