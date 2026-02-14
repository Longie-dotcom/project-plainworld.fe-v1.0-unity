using Assets.Service;
using Assets.UI.HUD.Console;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class ConsoleBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private ConsoleView consoleView;
    private ConsolePresenter consolePresenter;

    private UIService uiService;
    private ConsoleService consoleService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "HUD: Utils Console"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        yield return BindWhenReady<ConsoleService>(console =>
        {
            consoleService = console;
        });

        // Resolve dependencies
        consolePresenter = new ConsolePresenter(
            uiService,
            consoleService,
            consoleView);

        GameLogger.Info(
            Channel.System,
            "HUD: Utils Console components binded successfully");
    }

    private void OnDestroy()
    {
        consolePresenter?.Dispose();
    }
    #endregion
}
