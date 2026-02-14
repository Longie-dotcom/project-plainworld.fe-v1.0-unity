using Assets.Service;
using Assets.Service.Enum;
using System;

namespace Assets.UI.HUD.Setting
{
    public class SettingPresenter : IDisposable
    {
        #region Attributes
        private readonly SettingService settingService;
        private readonly SettingView settingView;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public SettingPresenter(
            SettingService settingService,
            SettingView settingView)
        {
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
            settingView.OnSmallScreenClicked -= OnSmallScreenClicked;
            settingView.OnMediumScreenClicked -= OnMediumScreenClicked;
            settingView.OnFullScreenClicked -= OnFullScreenClicked;

            // Outbound
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SettingPresenter));

            // Inbound
            settingView.OnSmallScreenClicked += OnSmallScreenClicked;
            settingView.OnMediumScreenClicked += OnMediumScreenClicked;
            settingView.OnFullScreenClicked += OnFullScreenClicked;

            // Outbound
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
        #endregion
        #endregion
    }
}
