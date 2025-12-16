using Assets.Core;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class AuthService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        #endregion

        public AuthService() { }

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

        public Task Login(string email, string password)
        {
            return Task.CompletedTask;
        }
        #endregion
    }
}
