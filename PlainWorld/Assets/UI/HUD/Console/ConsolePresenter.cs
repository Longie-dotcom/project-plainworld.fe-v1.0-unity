using Assets.Service;
using Assets.Utility;
using System;

namespace Assets.UI.HUD.Console
{
    public class ConsolePresenter : IDisposable
    {
        #region Attributes
        private UIService uiService;
        private ConsoleService consoleService;
        private ConsoleView consoleView;

        private string inputText;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public ConsolePresenter(
            UIService uiService,
            ConsoleService consoleService,
            ConsoleView consoleView)
        {
            this.uiService = uiService;
            this.consoleService = consoleService;
            this.consoleView = consoleView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            consoleView.OnSendClicked -= OnSendChatClicked;
            consoleView.OnInputChanged -= OnInputChanged;

            // Outbound
            uiService.UIState.OnUIStateChanged -= consoleView.HandleUIState;
            consoleService.ConsoleState.OnReceivedChat -= consoleView.AppendMessage;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(ConsolePresenter));

            // Inbound
            consoleView.OnSendClicked += OnSendChatClicked;
            consoleView.OnInputChanged += OnInputChanged;

            // Outbound
            uiService.UIState.OnUIStateChanged += consoleView.HandleUIState;
            consoleService.ConsoleState.OnReceivedChat += consoleView.AppendMessage;
        }

        #region Buttons
        private void OnSendChatClicked()
        {
            AsyncHelper.Run(async () =>
            {
                await consoleService.PlayerChat(inputText);
            });
        }
        #endregion

        #region Inputs
        private void OnInputChanged(string text)
        {
            inputText = text;
        }
        #endregion
        #endregion
    }
}
