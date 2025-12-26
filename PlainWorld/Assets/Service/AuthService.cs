using Assets.Core;
using Assets.Network.Interface.Command;
using Assets.Utility;
using System;
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

            // Parse claims
            Claims = JwtHelper.ParseClaims(
                JwtHelper.DecodePayload(
                    result.payload.accessToken));
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
    }
}
