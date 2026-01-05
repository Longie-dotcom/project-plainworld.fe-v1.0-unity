using Assets.Service;
using Assets.UI.MainMenu.Login;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class SettingBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private SettingView settingView;
    private SettingPresenter settingPresenter;

    private UIService uiService;
    private GameService gameService;
    private SettingService settingService;
    #endregion

    #region Properties
    public override string StepName 
    { 
        get { return "Setting UI"; } 
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

        yield return BindWhenReady<SettingService>(setting =>
        {
            settingService = setting;
        });

        // Resolve dependencies
        settingPresenter = new SettingPresenter(
            uiService,
            gameService,
            settingService,
            settingView);

        GameLogger.Info(
            Channel.System,
            "Setting UI components binded successfully");
    }

    private void OnDestroy()
    {
        settingPresenter?.Dispose();
    }
    #endregion
}
