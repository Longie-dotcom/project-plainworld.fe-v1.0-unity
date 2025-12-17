using Assets.Service;
using Assets.UI.MainMenu.Login;
using Assets.Utility;
using System;
using UnityEngine;

public class PlayerPresenter : IDisposable
{
    #region Attributes
    private readonly PlayerService playerService;

    private PlayerView view;
    private bool disposed;
    #endregion

    #region Properties
    #endregion

    public PlayerPresenter(PlayerService playerService)
    {
        this.playerService = playerService;
    }

    #region Methods
    public void Bind(PlayerView view)
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(LoginPresenter));

        this.view = view;

        view.OnJoinPressed += OnJoin;
        view.OnMove += OnMove;

        playerService.State.OnPositionChanged += view.ApplyPosition;
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;

        view.OnJoinPressed -= OnJoin;
        view.OnMove -= OnMove;

        playerService.State.OnPositionChanged -= view.ApplyPosition;
    }

    private void OnJoin()
    {
        AsyncHelper.Run(() => playerService.JoinAsync());
    }

    private void OnMove(Vector2 dir)
    {
        AsyncHelper.Run(() => playerService.MoveAsync(dir));
    }
    #endregion
}
