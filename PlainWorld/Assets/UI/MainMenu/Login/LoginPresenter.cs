using Assets.Service;
using Assets.State;
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
        private readonly LoginView loginPrefab;

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
            LoginView loginPrefab)
        {
            this.authService = authService;
            this.playerService = playerService;
            this.uiService = uiService;
            this.gameService = gameService;
            this.loginPrefab = loginPrefab;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            loginPrefab.OnEmailChanged -= OnEmailChanged;
            loginPrefab.OnPasswordChanged -= OnPasswordChanged;
            loginPrefab.OnJoinClicked -= OnLogin;
            loginPrefab.OnRegisterClicked -= OnRegister;

            uiService.UIState.OnUIStateChanged -= loginPrefab.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LoginPresenter));

            loginPrefab.OnEmailChanged += OnEmailChanged;
            loginPrefab.OnPasswordChanged += OnPasswordChanged;
            loginPrefab.OnJoinClicked += OnLogin;
            loginPrefab.OnRegisterClicked += OnRegister;

            uiService.UIState.OnUIStateChanged += loginPrefab.HandleUIState;
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
                // Authenticate player
                await authService.Login(email, password);
                var claims = authService.Claims;

                // Request Game Service to load player data
                await playerService.JoinAsync(
                    Guid.Parse(claims.UserId),
                    claims.FullName
                );
            });

            gameService.GameState.SetPhase(GamePhase.InGame);
        }

        private void OnRegister()
        {
            gameService.GameState.SetPhase(GamePhase.Register);
        }
        #endregion
    }
}
