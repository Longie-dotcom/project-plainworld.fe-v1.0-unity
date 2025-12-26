using Assets.Service;
using Assets.UI.Common.Popup;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class PopUpBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private PopUpView popUpPrefab;
    private PopUpPresenter popUpPresenter;

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
        });

        // Resolve dependencies
        popUpPresenter = new PopUpPresenter(
            uiService,
            popUpPrefab);

        GameLogger.Info(
            Channel.System,
            "Pop Up UI components binded successfully");
    }

    private void OnDestroy()
    {
        popUpPresenter?.Dispose();
    }
    #endregion
}
