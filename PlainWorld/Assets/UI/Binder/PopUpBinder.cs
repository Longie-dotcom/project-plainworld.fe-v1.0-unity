using Assets.Service;
using Assets.UI.Common.Popup;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class PopUpBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private PopUpView popUpView;
    private PopUpPresenter popUpPresenter;

    private UIService uiService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Popup UI"; }
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
        popUpPresenter = new PopUpPresenter(
            uiService,
            popUpView);

        GameLogger.Info(
            Channel.System,
            "Pop-up UI components binded successfully");
    }

    private void OnDestroy()
    {
        popUpPresenter?.Dispose();
    }
    #endregion
}
