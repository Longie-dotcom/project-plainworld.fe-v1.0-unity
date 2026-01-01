using Assets.Service.Interface;
using Assets.Network.Interface.Command;
using Assets.State;
using System.Threading.Tasks;
using Assets.State.Interface.IReadOnlyState;

namespace Assets.Service
{
    public class AuthService : IService
    {
        #region Attributes
        private readonly AuthState authState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IAuthNetworkCommand AuthNetworkCommand { get; private set; }
        public IReadOnlyAuthState AuthState { get { return authState; } }
        #endregion

        public AuthService()
        {
            authState = new AuthState();
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

        public void BindNetworkCommand(IAuthNetworkCommand command)
        {
            AuthNetworkCommand = command;
        }

        #region Senders
        public async Task Login(
            string email, 
            string password)
        {
            var result = await AuthNetworkCommand.Login(
                email, 
                password);

            // Parse claims
            authState.Set(result.payload.accessToken);
        }

        public async Task Register(
            string email,
            string password,
            string fullName,
            string gender,
            string dob)
        {
            await AuthNetworkCommand.Register(
                email, 
                password, 
                fullName,
                gender,
                dob);
        }
        #endregion

        #region Receivers
        #endregion
        #endregion
    }
}
