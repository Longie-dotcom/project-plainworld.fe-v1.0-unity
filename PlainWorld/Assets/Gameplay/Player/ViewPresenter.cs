using Assets.Service;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class ViewPresenter
{
    #region Attributes
    private readonly PlayerService playerService;

    private Guid playerId;
    private string playerName;
    #endregion

    #region Properties
    public bool HasJoined { get; private set; }
    public Vector2 Position { get; private set; }
    #endregion

    public ViewPresenter(
        PlayerService playerService,
        Guid playerId,
        string playerName)
    {
        this.playerService = playerService;
        this.playerId = playerId;
        this.playerName = playerName;
    }

    #region Methods
    public async Task Join()
    {
        HasJoined = true;

        await playerService.PlayerNetworkCommand.Join(
            playerId, playerName);
    }

    public async Task Move(Vector2 dir)
    {
        Position += dir * 5f;

        await playerService.PlayerNetworkCommand.Move(
            playerId, Position);
    }

    public void UpdatePosition(Vector2 dir)
    {
        Position = dir;
    }
    #endregion
}
