using Assets.Network.NetworkException;
using Assets.Service;
using Assets.Service.Enum;
using Assets.UI.Enum;
using Assets.Utility;
using System;

namespace Assets.UI.MainMenu.Login
{
    public class LoginPresenter : IDisposable
    {
        #region Attributes
        private readonly UIService uiService;
        private readonly GameService gameService;
        private readonly LoginView loginView;

        private string email;
        private string password;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public LoginPresenter(
            UIService uiService, 
            GameService gameService,
            LoginView loginView)
        {
            this.uiService = uiService;
            this.gameService = gameService;
            this.loginView = loginView;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            loginView.OnJoinClicked -= OnLoginClicked;
            loginView.OnRegisterClicked -= OnRegisterClicked;
            loginView.OnSettingClicked -= OnSettingClicked;

            loginView.OnEmailChanged -= OnEmailChanged;
            loginView.OnPasswordChanged -= OnPasswordChanged;

            // Outbound
            uiService.UIState.OnUIStateChanged -= loginView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LoginPresenter));

            // Inbound
            loginView.OnJoinClicked += OnLoginClicked;
            loginView.OnRegisterClicked += OnRegisterClicked;
            loginView.OnSettingClicked += OnSettingClicked;

            loginView.OnEmailChanged += OnEmailChanged;
            loginView.OnPasswordChanged += OnPasswordChanged;

            // Outbound
            uiService.UIState.OnUIStateChanged += loginView.HandleUIState;
        }

        #region Buttons
        private void OnLoginClicked()
        {
            AsyncHelper.Run(async () =>
            {
                try
                {
                    // Player login is a player life-cycle phase
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

        private void OnRegisterClicked()
        {
            gameService.PushPhase(GamePhase.Register);
        }

        private void OnSettingClicked()
        {
            gameService.PushPhase(GamePhase.Setting);
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
