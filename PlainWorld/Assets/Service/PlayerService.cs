using Assets.Core;
using Assets.Network.Interface.Command;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Service
{
    public class PlayerService : IService
    {
        #region Attributes
        private ViewPresenter viewLogic;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IPlayerNetworkCommand PlayerNetworkCommand { get; private set; }
        public Guid PlayerID { get; private set; } = Guid.Parse("a5a5405a-c1e1-49af-a68c-7cbb035be75d");

        public ViewPresenter Logic
        {
            get { return viewLogic; }
        }
        #endregion

        public PlayerService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (PlayerNetworkCommand == null)
                throw new InvalidOperationException(
                    "PlayerNetworkCommand not bound before Initialize");

            viewLogic = new ViewPresenter(
                this,
                PlayerID,
                "Long"
            );

            IsInitialized = true;

            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IPlayerNetworkCommand command)
        {
            PlayerNetworkCommand = command;
        }

        public void HandleUpdatePosition(float x, float y)
        {
            viewLogic.UpdatePosition(new Vector2(x, y));
        }
        #endregion
    }
}
