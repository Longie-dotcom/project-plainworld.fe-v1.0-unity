using Assets.Service;
using Assets.UI.MainMenu.Login;
using Assets.Utility;
using System;
using System.Collections;
using UnityEngine;

public class LoginBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private LoginView loginView;
    private LoginPresenter loginPresenter;
    private AuthService authService;
    private PlayerService playerService;
    private UIService uiService;
    private GameService gameService;
    #endregion

    #region Properties
    public LoginPresenter LoginPresenter
    { 
        get { return loginPresenter; } 
    }

    public event Action<LoginPresenter> OnLoginPresenterReady;
    #endregion

    public LoginBinder() { }

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<AuthService>(auth =>
        {
            authService = auth;
        });

        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        yield return BindWhenReady<GameService>(game =>
        {
            gameService = game;
        });

        // Resolve dependencies
        loginPresenter = new LoginPresenter(
            authService,
            playerService,
            uiService,
            gameService,
            playerPrefab);
        loginPresenter.Bind(loginView);

        // Invoke player binder to get the login presenter
        // Player presenter get player instance through login presenter event
        OnLoginPresenterReady?.Invoke(loginPresenter);

        GameLogger.Info(
            Channel.System,
            "Login UI components binded successfully");
    }

    private void OnDestroy()
    {
        loginPresenter?.Dispose();
    }
    #endregion
}
