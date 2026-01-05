using Assets.Service.Enum;
using Assets.State.Interface.IReadOnlyState;
using System;
using UnityEngine;

namespace Assets.State
{
    public class SettingState : IReadOnlySettingState
    {
        #region Attributes
        // Visual
        public float AnimationSpeedMultiplier { get; private set; }

        // Input / Networking
        public float MoveSendRate { get; private set; }

        // Screen
        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }
        public bool Fullscreen { get; private set; }

        // Events
        public event Action<IReadOnlySettingState> OnChanged;
        #endregion

        #region Properites
        #endregion

        public SettingState()
        {
            AnimationSpeedMultiplier = 8 / 5f;
            MoveSendRate = 0.01f;
        }

        #region Methods
        public void SetAnimationSpeedMultiplier(float value)
        {
            AnimationSpeedMultiplier = Mathf.Max(0.1f, value);
            OnChanged?.Invoke(this);
        }

        public void SetMoveSendRate(float value)
        {
            MoveSendRate = Mathf.Clamp(value, 0.005f, 0.1f);
            OnChanged?.Invoke(this);
        }

        public void SetScreenPreset(ScreenPreset preset)
        {
            switch (preset)
            {
                case ScreenPreset.Small:
                    ScreenWidth = 1280;
                    ScreenHeight = 720;
                    Fullscreen = false;
                    break;

                case ScreenPreset.Medium:
                    ScreenWidth = 1600;
                    ScreenHeight = 900;
                    Fullscreen = false;
                    break;

                case ScreenPreset.Full:
                    var res = Screen.currentResolution;
                    ScreenWidth = res.width;
                    ScreenHeight = res.height;
                    Fullscreen = true;
                    break;
            }

            OnChanged?.Invoke(this);
        }
        #endregion
    }
}
