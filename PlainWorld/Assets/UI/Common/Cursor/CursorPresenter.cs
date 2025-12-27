using Assets.Service;
using Assets.UI.Enum;
using System;

namespace Assets.UI.Common.Popup
{
    public class CursorPresenter : IDisposable
    {
        #region Attributes
        private readonly CursorService cursorService;
        private readonly CursorView cursorView;

        private bool disposed;
        #endregion

        #region Properties

        #endregion

        public CursorPresenter(
            CursorService cursorService,
            CursorView cursorView)
        {
            this.cursorService = cursorService;
            this.cursorView = cursorView;

            Bind();
            cursorView.Apply(CursorType.Default);
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            cursorService.CursorState.OnChanged -= cursorView.Apply;
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
            cursorService.CursorState.OnChanged += cursorView.Apply;
        }
        #endregion
    }
}
