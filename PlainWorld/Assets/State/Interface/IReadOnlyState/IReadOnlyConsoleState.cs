using System;

namespace Assets.State.Interface.IReadOnlyState
{
    public interface IReadOnlyConsoleState
    {
        event Action<string> OnReceivedChat;
    }
}
