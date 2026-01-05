using Assets.Network.Interface.Command;
using Assets.Service.Enum;
using Assets.Service.Interface;
using Assets.State;
using Assets.State.Interface.IReadOnlyState;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Service
{
    public class SettingService : IService
    {
        #region Attributes
        private readonly SettingState settingState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public ISettingNetworkCommand SettingNetworkCommand { get; private set; }
        public IReadOnlySettingState SettingState { get { return settingState; } }
        #endregion

        public SettingService()
        {
            settingState = new SettingState();
            SetScreenPreset(ScreenPreset.Full);
        }

        #region Methods
        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(ISettingNetworkCommand command)
        {
            SettingNetworkCommand = command;
        }

        public void SetScreenPreset(ScreenPreset screenPreset)
        {
            settingState.SetScreenPreset(screenPreset);

            // Apply resolution
            Screen.SetResolution(
                SettingState.ScreenWidth,
                SettingState.ScreenHeight,
                SettingState.Fullscreen
            );

            // Update UI reference resolution
            Canvas.ForceUpdateCanvases();
        }

        #region Senders
        #endregion

        #region Receivers
        #endregion
        #endregion
    }
}
