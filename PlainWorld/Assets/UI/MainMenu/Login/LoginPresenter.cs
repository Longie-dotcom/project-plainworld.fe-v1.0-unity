using Assets.Service;
using System;

namespace Assets.UI.MainMenu.Login
{
    public class LoginPresenter : IDisposable
    {
        #region Attributes
        private readonly LoginView view;
        private readonly AuthService service;

        private string email;
        private string password;
        #endregion

        #region Properties
        #endregion

        public LoginPresenter(
            LoginView view, 
            AuthService service)
        {
            this.view = view;
            this.service = service;

            view.OnEmailChanged += OnEmailChanged;
            view.OnPasswordChanged += OnPasswordChanged;
            view.OnJoinClicked += OnLogin;
        }

        #region Methods
        public void Dispose()
        {
            view.OnEmailChanged -= OnEmailChanged;
            view.OnPasswordChanged -= OnPasswordChanged;
            view.OnJoinClicked -= OnLogin;
        }

        private void OnEmailChanged(string v)
        {
            email = v;
        }

        private void OnPasswordChanged(string v)
        {
            password = v;
        }

        private async void OnLogin()
        {
            view.SetInteractable(false);

            try
            {
                await service.Login(email, password);
            }
            finally
            {
                view.SetInteractable(true);
            }
        }
        #endregion
    }
}
