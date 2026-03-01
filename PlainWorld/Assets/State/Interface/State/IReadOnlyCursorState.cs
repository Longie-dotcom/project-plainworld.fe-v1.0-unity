using Assets.UI.Enum;
using System;

namespace Assets.State.Interface.State
{
    public interface IReadOnlyCursorState
    {
        CursorType Current { get; }

        event Action<CursorType> OnChanged;
    }
}
