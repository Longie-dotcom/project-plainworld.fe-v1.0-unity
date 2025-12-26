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
        private readonly PopUpView popUpPrefab;

        private bool disposed;
        private PopUpData? current;
        #endregion

        #region Properties
        #endregion

        public PopUpPresenter(
            UIService uiService,
            PopUpView popUpPrefab)
        {
            this.uiService = uiService;
            this.popUpPrefab = popUpPrefab;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            popUpPrefab.OnOkClicked -= OnOk;
            popUpPrefab.OnCancelClicked -= OnCancel;

            uiService.UIState.OnPopUpRequested -= OnPopUpRequested;
        }

        private void Bind()
        {
            popUpPrefab.OnOkClicked += OnOk;
            popUpPrefab.OnCancelClicked += OnCancel;

            uiService.UIState.OnPopUpRequested += OnPopUpRequested;
        }

        private void OnPopUpRequested((PopUpType type, string message) request)
        {
            current = new PopUpData(request.type, request.message);

            popUpPrefab.SetMessage(current.Value.Message);

            switch (current.Value.Type)
            {
                case PopUpType.Information:
                    popUpPrefab.ShowInformation();
                    break;

                case PopUpType.Error:
                    popUpPrefab.ShowError();
                    break;

                case PopUpType.Question:
                    popUpPrefab.ShowQuestion();
                    break;
            }
        }

        private void OnOk()
        {
            current = null;
            popUpPrefab.Hide();
        }

        private void OnCancel()
        {
            current = null;
            popUpPrefab.Hide();
        }
        #endregion
    }
}
