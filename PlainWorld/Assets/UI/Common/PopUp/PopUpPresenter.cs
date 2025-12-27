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

        private bool disposed;
        private PopUpData? current;
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

            popUpView.OnOkClicked -= OnOk;
            popUpView.OnCancelClicked -= OnCancel;

            uiService.UIState.OnPopUpRequested -= OnPopUpRequested;
        }

        private void Bind()
        {
            popUpView.OnOkClicked += OnOk;
            popUpView.OnCancelClicked += OnCancel;

            uiService.UIState.OnPopUpRequested += OnPopUpRequested;
        }

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

        private void OnOk()
        {
            current = null;
            popUpView.Hide();
        }

        private void OnCancel()
        {
            current = null;
            popUpView.Hide();
        }
        #endregion
    }
}
