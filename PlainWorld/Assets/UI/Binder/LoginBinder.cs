using Assets.Service;
using Assets.UI.MainMenu.Login;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class LoginBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private LoginView loginView;
    private LoginPresenter loginPresenter;

    private AuthService authService;
    private PlayerService playerService;
    private UIService uiService;
    private GameService gameService;
    #endregion

    #region Properties
    #endregion

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
            loginView);

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
