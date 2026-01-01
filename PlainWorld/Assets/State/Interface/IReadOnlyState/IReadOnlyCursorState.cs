using Assets.UI.Enum;
using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyCursorState
    {
        CursorType Current { get; }

        event Action<CursorType> OnChanged;
    }
}
