using Assets.Service;
using Assets.UI.Menu.SignUp;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class SignUpBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private SignUpView signUpView;
    private SignUpPresenter signUpPresenter;

    private UIService uiService;
    private GameService gameService;
    private AuthService authService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Sign Up UI"; }
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
        signUpPresenter = new SignUpPresenter(
            uiService,
            gameService,
            authService,
            signUpView);

        GameLogger.Info(
            Channel.System,
            "Sign Up UI components binded successfully");
    }

    private void OnDestroy()
    {
        signUpPresenter?.Dispose();
    }
    #endregion
}
