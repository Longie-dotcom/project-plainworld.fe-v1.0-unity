using Assets.Service;
using Assets.UI.Enum;
using System;

namespace Assets.UI.Common.Popup
{
    public class CursorPresenter : IDisposable
    {
        #region Attributes
        private readonly CursorService cursorService;
        private readonly CursorView cursorPrefab;

        private bool disposed;
        #endregion

        #region Properties

        #endregion

        public CursorPresenter(
            CursorService cursorService,
            CursorView cursorPrefab)
        {
            this.cursorService = cursorService;
            this.cursorPrefab = cursorPrefab;

            Bind();
            cursorPrefab.Apply(CursorType.Default);
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            cursorService.CursorState.OnChanged -= cursorPrefab.Apply;
        }

        // Target objects
        public void BindTarget(CursorTarget target)
        {
            target.Bind(cursorService.CursorState.Set);
        }

        public void UnbindTarget(CursorTarget target)
        {
            target.Bind(null);
        }


        // Cursor (Source)
        private void Bind()
        {
            cursorService.CursorState.OnChanged += cursorPrefab.Apply;
        }
        #endregion
    }
}
