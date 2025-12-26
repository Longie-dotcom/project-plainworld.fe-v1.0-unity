using Assets.Gameplay.Player;
using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class PlayerBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private PlayerMoveView playerPrefab;
    private PlayerPresenter playerPresenter;

    private PlayerService playerService;
    private AuthService authService;
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

        yield return BindWhenReady<AuthService>(auth =>
        {
            authService = auth;
        });

        // Resolve dependencies
        playerPresenter = new PlayerPresenter(
            playerService,
            authService,
            playerPrefab);

        GameLogger.Info(
            Channel.System,
            "Player components bound successfully");
    }


    private void OnDestroy()
    {
        playerPresenter?.Dispose();
    }
    #endregion
}
