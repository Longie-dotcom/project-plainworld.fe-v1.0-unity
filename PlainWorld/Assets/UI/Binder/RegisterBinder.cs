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
    private AuthService authService;
    private UIService uiService;
    private GameService gameService;
    #endregion

    #region Properties
    #endregion

    public RegisterBinder() { }

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<AuthService>(auth =>
        {
            authService = auth;
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
        registerPresenter = new RegisterPresenter(
            authService,
            uiService,
            gameService);
        registerPresenter.Bind(registerView);

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
