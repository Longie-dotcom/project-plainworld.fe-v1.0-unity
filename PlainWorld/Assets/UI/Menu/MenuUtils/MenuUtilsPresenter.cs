using Assets.Service;
using System;

namespace Assets.UI.Menu.MenuUtils
{
    public class MenuUtilsPresenter : IDisposable
    {
        #region Attributes
        private readonly UIService uiService;
        private readonly MenuUtilsView menuUtilsView;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public MenuUtilsPresenter(
            UIService uiService,
            MenuUtilsView menuUtilsView)
        {
            this.uiService = uiService;
            this.menuUtilsView = menuUtilsView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound

            // Outbound
            uiService.UIState.OnUIStateChanged -= menuUtilsView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(MenuUtilsPresenter));

            // Inbound

            // Outbound
            uiService.UIState.OnUIStateChanged += menuUtilsView.HandleUIState;
        }
        #endregion
    }
}
