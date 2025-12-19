using Assets.Gameplay.Player;
using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class PlayerBinder : ComponentBinder
{
    #region Attributes
    private LoginBinder loginBinder;

    private PlayerPresenter playerPresenter;
    private PlayerService playerService;
    #endregion

    #region Properties
    #endregion

    public PlayerBinder() { }

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        // Resolve dependencies
        loginBinder = GetComponent<LoginBinder>();
        loginBinder.OnLoginPresenterReady += presenter =>
        {
            presenter.OnPlayerCreated += BindPlayerView;
        };

        GameLogger.Info(
            Channel.System,
            "Player components bound successfully");
    }

    private void BindPlayerView(GameObject playerInstance)
    {
        playerPresenter = new PlayerPresenter(playerService);
        playerPresenter.Bind(
            playerInstance.GetComponent<PlayerMoveView>());

        GameLogger.Info(
            Channel.System,
            "Binding player presenter successfully");
    }

    private void OnDestroy()
    {
        if (loginBinder?.LoginPresenter != null)
            loginBinder.LoginPresenter.OnPlayerCreated -= BindPlayerView;

        playerPresenter?.Dispose();
    }
    #endregion
}
