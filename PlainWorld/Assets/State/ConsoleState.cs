using Assets.State.Interface.IReadOnlyState;
using System;

namespace Assets.State
{
    public class ConsoleState : IReadOnlyConsoleState
    {
        #region Attributes
        #endregion

        #region Properites
        public event Action<string> OnReceivedChat;
        #endregion

        public ConsoleState()
        {

        }

        #region Methods
        public void OnPlayerChatted(
            string userName,
            string message)
        {
            AppendChat(userName, message);
        }

        public void OnPlayerEntityChatted(
            string userName,
            string message)
        {
            AppendChat(userName, message);
        }

        private void AppendChat(
            string userName,
            string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            OnReceivedChat?.Invoke($"{userName}: {message}");
        }
        #endregion
    }
}
