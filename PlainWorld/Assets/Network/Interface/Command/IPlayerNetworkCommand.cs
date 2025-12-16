using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Network.Interface.Command
{
    public interface IPlayerNetworkCommand
    {
        Task Join(Guid playerId, string playerName);
        Task Move(Guid playerId, Vector2 position);
    }
}
