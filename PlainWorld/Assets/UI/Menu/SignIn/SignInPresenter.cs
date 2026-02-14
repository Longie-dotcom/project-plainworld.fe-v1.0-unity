using Assets.Network.NetworkException;
using Assets.Service;
using Assets.UI.Enum;
using Assets.Utility;
using System;

namespace Assets.UI.Menu.SignIn
{
    public class SignInPresenter : IDisposable
    {
        #region Attributes
        private readonly UIService uiService;
        private readonly GameService gameService;
        private readonly SignInView signInView;

        private string email;
        private string password;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public SignInPresenter(
            UIService uiService, 
            GameService gameService,
            SignInView signInView)
        {
            this.uiService = uiService;
            this.gameService = gameService;
            this.signInView = signInView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            signInView.OnSignInClicked -= OnSignInClicked;

            signInView.OnEmailChanged -= OnEmailChanged;
            signInView.OnPasswordChanged -= OnPasswordChanged;

            // Outbound
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(SignInPresenter));

            // Inbound
            signInView.OnSignInClicked += OnSignInClicked;

            signInView.OnEmailChanged += OnEmailChanged;
            signInView.OnPasswordChanged += OnPasswordChanged;

            // Outbound
        }

        #region Buttons
        private void OnSignInClicked()
        {
            AsyncHelper.Run(async () =>
            {
                try
                {
                    // Player sign in is a player life-cycle phase
                    await gameService.PlayerLogin(email, password);
                }
                catch (AuthException ex)
                {
                    uiService.ShowPopUp(
                        PopUpType.Error,
                        ex.Message
                    );
                }
                catch (Exception)
                {
                    uiService.ShowPopUp(
                        PopUpType.Error,
                        "Something went wrong. Please try again."
                    );
                }
            });
        }
        #endregion

        #region Inputs
        private void OnEmailChanged(string v)
        {
            email = v;
        }

        private void OnPasswordChanged(string v)
        {
            password = v;
        }
        #endregion
        #endregion
    }
}
