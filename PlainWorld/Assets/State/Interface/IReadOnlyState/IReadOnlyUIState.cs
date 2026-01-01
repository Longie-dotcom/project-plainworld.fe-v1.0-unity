using Assets.UI.Enum;
using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyUIState
    {
        bool ShowLogin { get; }
        bool ShowRegister { get; }
        bool ShowCustomizeCharacter { get; }
        bool ShowHUD { get; }

        bool ShowLobby { get; }
        bool ShowLoading { get; }

        event Action<UIState> OnUIStateChanged;
        event Action<(PopUpType type, string message)> OnPopUpRequested;
    }
}
