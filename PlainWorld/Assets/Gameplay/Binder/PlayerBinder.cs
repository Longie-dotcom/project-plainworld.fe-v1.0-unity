using Assets.Service;
using System.Collections;
using UnityEngine;

public class PlayerBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private PlayerView playerView;
    private ViewPresenter presenter;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private IEnumerator Start()
    {
        // Use the generic BindWhenReady from ComponentBinder
        yield return BindWhenReady<PlayerService>(playerService =>
        {
            // Create presenter for this prefab instance
            presenter = new ViewPresenter(
                playerService, playerService.PlayerID, "Long");

            // Assign presenter to the View
            playerView.SetLogic(presenter);

            // Optionally subscribe to service events
            // playerService.OnPositionUpdate += presenter.UpdatePosition;
        });
    }

    private void OnDestroy()
    {
        presenter = null;
    }
    #endregion
}
