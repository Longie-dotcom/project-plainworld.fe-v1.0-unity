using Assets.UI.Enum;
using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyUIState
    {
        bool ShowMenu { get; }
        bool ShowHUD { get; }
        bool ShowLoading { get; }

        event Action<UIState> OnUIStateChanged;
        event Action<(PopUpType type, string message)> OnPopUpRequested;
    }
}
