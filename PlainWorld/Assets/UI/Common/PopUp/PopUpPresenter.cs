using Assets.Service;
using Assets.UI.Enum;
using System;

namespace Assets.UI.Common.Popup
{
    public readonly struct PopUpData
    {
        public readonly PopUpType Type;
        public readonly string Message;

        public PopUpData(PopUpType type, string message)
        {
            Type = type;
            Message = message;
        }
    }

    public class PopUpPresenter : IDisposable
    {
        #region Attributes
        private readonly UIService uiService;
        private readonly PopUpView popUpView;

        private PopUpData? current;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public PopUpPresenter(
            UIService uiService,
            PopUpView popUpView)
        {
            this.uiService = uiService;
            this.popUpView = popUpView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            popUpView.OnOkClicked -= OnOkClicked;
            popUpView.OnCancelClicked -= OnCancelClicked;

            // Outbound
            uiService.UIState.OnPopUpRequested -= OnPopUpRequested;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(PopUpPresenter));

            // Inbound
            popUpView.OnOkClicked += OnOkClicked;
            popUpView.OnCancelClicked += OnCancelClicked;

            // Outbound
            uiService.UIState.OnPopUpRequested += OnPopUpRequested;
        }

        #region Buttons
        private void OnOkClicked()
        {
            current = null;
            popUpView.Hide();
        }

        private void OnCancelClicked()
        {
            current = null;
            popUpView.Hide();
        }
        #endregion

        #region Outbound
        private void OnPopUpRequested((PopUpType type, string message) request)
        {
            current = new PopUpData(request.type, request.message);

            popUpView.SetMessage(current.Value.Message);

            switch (current.Value.Type)
            {
                case PopUpType.Information:
                    popUpView.ShowInformation();
                    break;

                case PopUpType.Error:
                    popUpView.ShowError();
                    break;

                case PopUpType.Question:
                    popUpView.ShowQuestion();
                    break;
            }
        }
        #endregion
        #endregion
    }
}
