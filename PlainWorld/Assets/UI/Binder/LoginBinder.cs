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

    private UIService uiService;
    private GameService gameService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Login UI"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
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
