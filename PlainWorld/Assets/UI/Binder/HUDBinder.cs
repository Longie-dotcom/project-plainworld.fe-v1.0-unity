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
    private GameService gameService;
    private UIService uiService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "HUD UI"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        yield return BindWhenReady<GameService>(game =>
        {
            gameService = game;
        });

        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        // Resolve dependencies
        hudPresenter = new HUDPresenter(
            playerService,
            gameService,
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
