using Assets.Network;
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
        private readonly NetworkService networkService;
        private readonly AuthService authService;
        private readonly PlayerService playerService;
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
            NetworkService networkService,
            AuthService authService,
            PlayerService playerService,
            UIService uiService, 
            GameService gameService,
            LoginView loginView)
        {
            this.networkService = networkService;
            this.authService = authService;
            this.playerService = playerService;
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
                    // Authenticate players
                    await authService.Login(email, password);

                    // Connect to the game service and start session
                    await networkService.ConnectAsync(authService.AuthState.Token);
                    await networkService.Session.StartSessionAsync();

                    // Request spawning players
                    await playerService.JoinAsync();
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
            gameService.SetPhase(GamePhase.Register);
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
