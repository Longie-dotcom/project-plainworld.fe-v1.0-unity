using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.Utility;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class AuthService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IAuthNetworkCommand AuthNetworkCommand { get; private set; }
        public JwtClaims Claims { get; private set; }
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

        public void BindNetworkCommand(IAuthNetworkCommand command)
        {
            AuthNetworkCommand = command;
        }

        public async Task Login(
            string email, 
            string password)
        {
            var result = await AuthNetworkCommand.Login(
                email, 
                password);
            var accessToken = result.payload.accessToken;
            var refreshToken = result.payload.refreshToken;

            // Decode JWT
            var jsonPayload = JwtHelper.DecodePayload(accessToken);

            // Parse claims
            Claims = JwtHelper.ParseClaims(jsonPayload);
        }

        public async Task Register(
            string email,
            string password,
            string fullName,
            string dob,
            string gender)
        {
            await AuthNetworkCommand.Register(
                email, 
                password, 
                fullName, 
                dob, 
                gender);
        }
        #endregion
    }
}
