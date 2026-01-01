using Assets.State.Interface.IReadOnlyState;
using Assets.UI.Enum;
using System;

namespace Assets.State
{
    public class CursorState : IReadOnlyCursorState
    {
        #region Attributes
        #endregion

        #region Properites
        public CursorType Current { get; private set; }

        public event Action<CursorType> OnChanged;
        #endregion

        public CursorState()
        {
            Set(CursorType.Default);
        }

        #region Methods
        public void Set(CursorType type)
        {
            if (Current == type) return;

            Current = type;
            OnChanged?.Invoke(type);
        }
        #endregion
    }
}
