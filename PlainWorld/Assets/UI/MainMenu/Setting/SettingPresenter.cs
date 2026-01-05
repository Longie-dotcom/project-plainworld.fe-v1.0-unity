using Assets.Service;
using Assets.Service.Enum;
using System;

namespace Assets.UI.MainMenu.Login
{
    public class SettingPresenter : IDisposable
    {
        #region Attributes
        private readonly UIService uiService;
        private readonly GameService gameService;
        private readonly SettingService settingService;
        private readonly SettingView settingView;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public SettingPresenter(
            UIService uiService,
            GameService gameService,
            SettingService settingService,
            SettingView settingView)
        {
            this.uiService = uiService;
            this.gameService = gameService;
            this.settingService = settingService;
            this.settingView = settingView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            settingView.OnBackClicked -= OnBackClicked;
            settingView.OnSmallScreenClicked -= OnSmallScreenClicked;
            settingView.OnMediumScreenClicked -= OnMediumScreenClicked;
            settingView.OnFullScreenClicked -= OnFullScreenClicked;

            // Outbound
            uiService.UIState.OnUIStateChanged -= settingView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SettingPresenter));

            // Inbound
            settingView.OnBackClicked += OnBackClicked;
            settingView.OnSmallScreenClicked += OnSmallScreenClicked;
            settingView.OnMediumScreenClicked += OnMediumScreenClicked;
            settingView.OnFullScreenClicked += OnFullScreenClicked;

            // Outbound
            uiService.UIState.OnUIStateChanged += settingView.HandleUIState;
        }

        #region Buttons
        private void OnSmallScreenClicked()
        {
            settingService.SetScreenPreset(ScreenPreset.Small);
        }

        private void OnMediumScreenClicked()
        {
            settingService.SetScreenPreset(ScreenPreset.Medium);
        }

        private void OnFullScreenClicked()
        {
            settingService.SetScreenPreset(ScreenPreset.Full);
        }

        private void OnBackClicked()
        {
            gameService.PopPhase();
        }
        #endregion
        #endregion
    }
}
