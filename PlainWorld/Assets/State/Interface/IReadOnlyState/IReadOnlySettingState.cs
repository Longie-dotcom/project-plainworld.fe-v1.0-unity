using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlySettingState
    {
        // Visual
        float AnimationSpeedMultiplier { get; }

        // Input / Networking
        float MoveSendRate { get; }

        // Screen
        int ScreenWidth { get; }
        int ScreenHeight { get; }
        bool Fullscreen { get; }

        event Action<IReadOnlySettingState> OnChanged;
    }
}
