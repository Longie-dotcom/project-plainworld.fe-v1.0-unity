using Assets.Service;
using Assets.UI.GameMenu.HUD;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class HUDBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private HUDView hudView;
    private HUDPresenter hudPresenter;

    private PlayerService playerService;
    private UIService uiService;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private IEnumerator Start()
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
        hudPresenter = new HUDPresenter(
            playerService,
            uiService,
            hudView);

        GameLogger.Info(
            Channel.System,
            "HUD UI components binded successfully");
    }

    private void OnDestroy()
    {
        hudPresenter?.Dispose();
    }
    #endregion
}
