using Assets.Service;
using Assets.UI.Menu.MenuUtils;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class MenuUtilsBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private MenuUtilsView menuUtilsView;
    private MenuUtilsPresenter menuUtilsPresenter;

    private UIService uiService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Menu: Utils UI"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        // Resolve dependencies
        menuUtilsPresenter = new MenuUtilsPresenter(
            uiService,
            menuUtilsView);

        GameLogger.Info(
            Channel.System,
            "Menu: Utils UI components binded successfully");
    }

    private void OnDestroy()
    {
        menuUtilsPresenter?.Dispose();
    }
    #endregion
}
