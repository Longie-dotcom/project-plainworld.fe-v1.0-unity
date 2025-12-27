using Assets.Service;
using Assets.UI.Common.Popup;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class CursorBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private CursorView cursorView;
    private CursorPresenter cursorPresenter;

    private CursorService cursorService;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<CursorService>(cursor =>
        {
            cursorService = cursor;
        });

        // Resolve dependencies
        cursorPresenter = new CursorPresenter(
            cursorService,
            cursorView);

        CursorTarget.OnTargetEnabled += cursorPresenter.BindTarget;
        CursorTarget.OnTargetDisabled += cursorPresenter.UnbindTarget;

        var targets = FindObjectsByType<CursorTarget>(
            FindObjectsSortMode.None);

        foreach (var target in targets)
        {
            if (target.isActiveAndEnabled)
            {
                cursorPresenter.BindTarget(target);
            }
        }
        
        GameLogger.Info(
            Channel.System,
            "Cursor components binded successfully");
    }

    private void OnDestroy()
    {
        cursorPresenter?.Dispose();
    }
    #endregion
}
