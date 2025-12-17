using Assets.Service;
using Assets.UI.MainMenu.Login;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class LoginUIBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private LoginView loginView;
    private LoginPresenter presenter;
    private UIService uiService;
    private AuthService authService;
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

        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        // Resolve dependencies
        presenter = new LoginPresenter(authService, uiService);
        presenter.Bind(loginView);

        GameLogger.Info(
            Channel.System,
            "Login UI components binded successfully");
    }

    private void OnDestroy()
    {
        presenter?.Dispose();
    }
    #endregion
}
