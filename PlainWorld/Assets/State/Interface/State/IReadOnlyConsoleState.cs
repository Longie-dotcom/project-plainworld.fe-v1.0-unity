using System;

namespace Assets.State.Interface.State
{
    public interface IReadOnlyConsoleState
    {
        event Action<string> OnReceivedChat;
    }
}
