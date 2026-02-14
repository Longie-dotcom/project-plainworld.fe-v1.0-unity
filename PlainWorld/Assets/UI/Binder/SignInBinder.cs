using Assets.Service;
using Assets.UI.Menu.SignIn;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class SignInBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private SignInView signInView;
    private SignInPresenter signInPresenter;

    private UIService uiService;
    private GameService gameService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Sign In UI"; }
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
        signInPresenter = new SignInPresenter(
            uiService,
            gameService,
            signInView);

        GameLogger.Info(
            Channel.System,
            "Sign In UI components binded successfully");
    }

    private void OnDestroy()
    {
        signInPresenter?.Dispose();
    }
    #endregion
}
