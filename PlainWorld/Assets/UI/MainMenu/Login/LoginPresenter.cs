using Assets.Network.NetworkException;
using Assets.Service;
using Assets.State;
using Assets.UI.Enum;
using Assets.Utility;
using System;

namespace Assets.UI.MainMenu.Login
{
    public class LoginPresenter : IDisposable
    {
        #region Attributes
        private readonly AuthService authService;
        private readonly PlayerService playerService;
        private readonly UIService uiService;
        private readonly GameService gameService;
        private readonly LoginView loginView;

        private bool disposed;
        private string email;
        private string password;
        #endregion

        #region Properties
        #endregion

        public LoginPresenter(
            AuthService authService,
            PlayerService playerService,
            UIService uiService, 
            GameService gameService,
            LoginView loginView)
        {
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

            loginView.OnEmailChanged -= OnEmailChanged;
            loginView.OnPasswordChanged -= OnPasswordChanged;
            loginView.OnJoinClicked -= OnLogin;
            loginView.OnRegisterClicked -= OnRegister;

            uiService.UIState.OnUIStateChanged -= loginView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LoginPresenter));

            loginView.OnEmailChanged += OnEmailChanged;
            loginView.OnPasswordChanged += OnPasswordChanged;
            loginView.OnJoinClicked += OnLogin;
            loginView.OnRegisterClicked += OnRegister;

            uiService.UIState.OnUIStateChanged += loginView.HandleUIState;
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
            AsyncHelper.Run(async () =>
            {
                try
                {
                    // Authenticate players
                    await authService.Login(email, password);
                    var claims = authService.Claims;

                    // Request spawning players
                    await playerService.JoinAsync(
                        Guid.Parse(claims.UserId),
                        claims.FullName
                    );
                    gameService.GameState.SetPhase(GamePhase.InGame);
                }
                catch (AuthException ex)
                {
                    uiService.UIState.ShowPopUp(
                        PopUpType.Error,
                        ex.Message
                    );
                }
                catch (Exception)
                {
                    uiService.UIState.ShowPopUp(
                        PopUpType.Error,
                        "Something went wrong. Please try again."
                    );
                }
            });
        }

        private void OnRegister()
        {
            gameService.GameState.SetPhase(GamePhase.Register);
        }
        #endregion
    }
}
