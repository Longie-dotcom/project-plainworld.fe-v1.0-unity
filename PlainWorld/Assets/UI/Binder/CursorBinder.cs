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
    public override string StepName
    {
        get { return "Cursor and Cursor Targets"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
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
            "Cursor and cursor targets components binded successfully");
    }

    private void OnDestroy()
    {
        cursorPresenter?.Dispose();
    }
    #endregion
}
