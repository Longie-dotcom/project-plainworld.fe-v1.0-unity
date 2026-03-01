using Assets.State.Interface.Component.Shared;

namespace Assets.State.Interface.Component.Entity
{
    public interface IReadOnlyGrayShroomEntity
    {
        IReadOnlyAct Act { get; }
    }
}
