using Assets.Service;
using Assets.Utility;
using System;

namespace Assets.UI.GameMenu.HUD
{
    public class HUDPresenter : IDisposable
    {
        #region Attributes
        private readonly PlayerService playerService;
        private readonly UIService uiService;
        private readonly HUDView hudView;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public HUDPresenter(
            PlayerService playerService,
            UIService uiService,
            HUDView hudView)
        {
            this.playerService = playerService;
            this.uiService = uiService;
            this.hudView = hudView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            hudView.OnLogoutClicked -= OnLogoutClicked;

            // Outbound
            uiService.UIState.OnUIStateChanged -= hudView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(HUDPresenter));

            // Inbound
            hudView.OnLogoutClicked += OnLogoutClicked;

            // Outbound
            uiService.UIState.OnUIStateChanged += hudView.HandleUIState;
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
