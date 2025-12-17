using Assets.Service;
using Assets.Utility;
using System;

namespace Assets.UI.MainMenu.Login
{
    public class LoginPresenter : IDisposable
    {
        #region Attributes
        private readonly AuthService authService;
        private readonly UIService uiService;

        private LoginView view;
        private bool disposed;

        private string email;
        private string password;
        #endregion

        #region Properties
        #endregion

        public LoginPresenter(AuthService authService, UIService uiService)
        {
            this.authService = authService;
            this.uiService = uiService;
        }

        #region Methods
        public void Bind(LoginView view)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LoginPresenter));

            this.view = view;

            view.OnEmailChanged += OnEmailChanged;
            view.OnPasswordChanged += OnPasswordChanged;
            view.OnJoinClicked += OnLogin;

            uiService.OnUIStateChanged += view.HandleUIState;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            view.OnEmailChanged -= OnEmailChanged;
            view.OnPasswordChanged -= OnPasswordChanged;
            view.OnJoinClicked -= OnLogin;

            uiService.OnUIStateChanged -= view.HandleUIState;
        }

        private void OnEmailChanged(string v)
        {
            email = v;
        }

        private void OnPasswordChanged(string v)
        {
            password = v;
        }

        private void OnLogin()
        {
            view.SetInteractable(false);
            AsyncHelper.Run(() => authService.Login(email, password));
        }
        #endregion
    }
}
