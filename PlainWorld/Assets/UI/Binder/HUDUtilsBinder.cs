using Assets.Service;
using Assets.UI.HUD.HUDUtils;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class HUDUtilsBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private HUDUtilsView hudUtilsView;
    private HUDUtilsPresenter hudUtilsPresenter;

    private PlayerService playerService;
    private UIService uiService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "HUD: Utils UI"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        // Resolve dependencies
        hudUtilsPresenter = new HUDUtilsPresenter(
            playerService,
            uiService,
            hudUtilsView);

        GameLogger.Info(
            Channel.System,
            "HUD: Utils UI components binded successfully");
    }

    private void OnDestroy()
    {
        hudUtilsPresenter?.Dispose();
    }
    #endregion
}
