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

            // Outbound
            consoleService.ConsoleState.OnReceivedChat -= consoleView.AppendMessage;
            uiService.UIState.OnUIStateChanged -= consoleView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(ConsolePresenter));

            // Inbound
            consoleView.OnSendClicked += OnSendChatClicked;

            // Outbound
            consoleService.ConsoleState.OnReceivedChat += consoleView.AppendMessage;
            uiService.UIState.OnUIStateChanged += consoleView.HandleUIState;
        }

        #region Buttons
        private void OnSendChatClicked(string message)
        {
            AsyncHelper.Run(async () => 
            { 
                await consoleService.PlayerChat(message); 
            });
        }
        #endregion
        #endregion
    }
}
