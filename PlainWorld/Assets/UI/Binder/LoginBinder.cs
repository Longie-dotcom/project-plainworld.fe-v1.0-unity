using Assets.Core;
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
    #endregion

    #region Properties
    #endregion

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;

            var auth = ServiceLocator.Get<AuthService>();
            presenter = new LoginPresenter(loginView, auth);

            ui.OnUIStateChanged += loginView.HandleUIState;

            GameLogger.Info(
                Channel.System,
                "Login UI components binded successfully");
        });
    }

    private void OnDestroy()
    {
        presenter?.Dispose();

        if (uiService != null)
            uiService.OnUIStateChanged -= loginView.HandleUIState;
    }
    #endregion
}
