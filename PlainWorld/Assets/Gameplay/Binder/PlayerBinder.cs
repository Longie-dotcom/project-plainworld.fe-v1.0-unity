using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class PlayerBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private PlayerView playerView;
    private PlayerService playerService;
    private PlayerPresenter presenter;
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

        // Resolve dependencies
        presenter = new PlayerPresenter(playerService);
        presenter.Bind(playerView);

        GameLogger.Info(
            Channel.System,
            "Player components bound successfully");
    }

    private void OnDestroy()
    {
        presenter?.Dispose();
    }
    #endregion
}
