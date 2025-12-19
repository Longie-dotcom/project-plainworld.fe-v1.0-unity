using Assets.Service;
using Assets.State;
using Assets.Utility;
using System;
using UnityEngine;

namespace Assets.UI.MainMenu.Login
{
    public class LoginPresenter : IDisposable
    {
        #region Attributes
        private readonly AuthService authService;
        private readonly PlayerService playerService;
        private readonly UIService uiService;
        private readonly GameService gameService;
        private readonly GameObject playerPrefab;

        private LoginView loginView;
        private bool disposed;

        private string email;
        private string password;
        #endregion

        #region Properties
        public event Action<GameObject> OnPlayerCreated;
        #endregion

        public LoginPresenter(
            AuthService authService,
            PlayerService playerService,
            UIService uiService, 
            GameService gameService,
            GameObject playerPrefab)
        {
            this.authService = authService;
            this.playerService = playerService;
            this.uiService = uiService;
            this.gameService = gameService;
            this.playerPrefab = playerPrefab;
        }

        #region Methods
        public void Bind(LoginView loginView)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LoginPresenter));

            this.loginView = loginView;

            loginView.OnEmailChanged += OnEmailChanged;
            loginView.OnPasswordChanged += OnPasswordChanged;
            loginView.OnJoinClicked += OnLogin;
            loginView.OnRegisterClicked += OnRegister;

            uiService.UIState.OnUIStateChanged += loginView.HandleUIState;
        }

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

                // Instantiate player
                var playerInstance = GameObject.Instantiate(playerPrefab);
                playerInstance.name = $"MainPlayer_{claims.FullName}";

                // Notify listeners (e.g., PlayerBinder)
                OnPlayerCreated?.Invoke(playerInstance);
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
