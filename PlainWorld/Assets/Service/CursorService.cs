using Assets.Service.Interface;
using Assets.Network.Interface.Command;
using Assets.State;
using System.Threading.Tasks;
using Assets.State.Interface.IReadOnlyState;
using Assets.UI.Enum;

namespace Assets.Service
{
    public class CursorService : IService
    {
        #region Attributes
        private readonly CursorState cursorState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public ICursorNetworkCommand CursorNetworkCommand { get; private set; }
        public IReadOnlyCursorState CursorState { get { return cursorState; } }
        #endregion

        public CursorService()
        {
            cursorState = new CursorState();
        }

        #region Methods
        public Task InitializeAsync()
        {
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

        public void Set(CursorType cursorType)
        {
            cursorState.Set(cursorType);
        }

        #region Senders
        #endregion

        #region Receivers
        #endregion
        #endregion
    }
}
