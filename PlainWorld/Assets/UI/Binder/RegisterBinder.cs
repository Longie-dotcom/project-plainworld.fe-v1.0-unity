using Assets.Service;
using Assets.UI.MainMenu.Register;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class RegisterBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private RegisterView registerView;
    private RegisterPresenter registerPresenter;

    private UIService uiService;
    private GameService gameService;
    private AuthService authService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Register UI"; }
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

        yield return BindWhenReady<AuthService>(auth =>
        {
            authService = auth;
        });

        // Resolve dependencies
        registerPresenter = new RegisterPresenter(
            uiService,
            gameService,
            authService,
            registerView);

        GameLogger.Info(
            Channel.System,
            "Register UI components binded successfully");
    }

    private void OnDestroy()
    {
        registerPresenter?.Dispose();
    }
    #endregion
}
