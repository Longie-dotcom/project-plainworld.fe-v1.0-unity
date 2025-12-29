using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.State.Cursor;
using System;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class CursorService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public ICursorNetworkCommand CursorNetworkCommand { get; private set; }
        public CursorState CursorState { get; private set; } = new CursorState();
        #endregion

        public CursorService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (CursorNetworkCommand == null)
                throw new InvalidOperationException(
                    "CursorNetworkCommand not bound before Initialize");

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(ICursorNetworkCommand command)
        {
            CursorNetworkCommand = command;
        }
        #endregion
    }
}
