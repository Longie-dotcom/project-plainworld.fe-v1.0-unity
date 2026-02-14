using Assets.Service;
using Assets.Utility;
using System;

namespace Assets.UI.HUD.HUDUtils
{
    public class HUDUtilsPresenter : IDisposable
    {
        #region Attributes
        private readonly PlayerService playerService;
        private readonly UIService uiService;
        private readonly HUDUtilsView hudUtilsView;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public HUDUtilsPresenter(
            PlayerService playerService,
            UIService uiService,
            HUDUtilsView hudUtilsView)
        {
            this.playerService = playerService;
            this.uiService = uiService;
            this.hudUtilsView = hudUtilsView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            hudUtilsView.OnLogoutClicked -= OnLogoutClicked;

            // Outbound
            uiService.UIState.OnUIStateChanged -= hudUtilsView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(HUDUtilsPresenter));

            // Inbound
            hudUtilsView.OnLogoutClicked += OnLogoutClicked;

            // Outbound
            uiService.UIState.OnUIStateChanged += hudUtilsView.HandleUIState;
        }

        #region Buttons
        private void OnLogoutClicked()
        {
            AsyncHelper.Run(async () => await playerService.LogoutAsync());
        }
        #endregion
        #endregion
    }
}
