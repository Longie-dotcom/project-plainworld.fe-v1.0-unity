using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Network.NetworkException;
using Assets.Service;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Network.Handler
{
    public class AuthNetworkHandler :
        IAuthNetworkReceiver,
        IAuthNetworkCommand
    {
        #region Attributes
        private AuthService authService;
        private NetworkCommandSender sender = new();

        private const string BASE_URL = "http://26.92.115.30:7000/";
        private HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri(BASE_URL),
            Timeout = TimeSpan.FromSeconds(10)
        };
        #endregion

        #region Properties
        public string Group { get; private set; }
        #endregion

        public AuthNetworkHandler() { }

        #region Methods
        public void BindService(AuthService service, NetworkService network)
        {
            authService = service;
            sender.BindNetwork(network);
        }
        #endregion

        #region Send Commands
        #endregion

        #region Receive Handlers
        #endregion

        #region Http Requests
        public async Task<HttpResponse<TokenPayload>> Login(
            string email,
            string password)
        {
            var payload = new LoginRequest
            {
                email = email,
                password = password,
                roleCode = "NORMAL_USER"
            };

            var result = await PostAsync<LoginRequest, HttpResponse<TokenPayload>>(
                "iam/auth/login",
                payload);

            if (result?.payload == null ||
                string.IsNullOrEmpty(result.payload.accessToken))
            {
                throw new Exception("Login succeeded but token payload is invalid");
            }

            return result;
        }

        public async Task Register(
            string email,
            string password,
            string fullName,
            string gender,
            string dob)
        {
            var dto = new RegisterRequest
            {
                email = email,
                password = password,
                fullName = fullName,
                gender = gender,
                dob = dob
            };

            await PostAsync<RegisterRequest, object>(
                "iam/auth/register",
                dto);
        }
        #endregion

        #region Private Helpers
        private async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest payload)
        {
            var json = JsonUtility.ToJson(payload);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                string message = "Request failed";

                if (!string.IsNullOrWhiteSpace(body))
                {
                    try
                    {
                        var error = JsonUtility.FromJson<ErrorResponse>(body);
                        if (!string.IsNullOrEmpty(error.message))
                            message = error.message;
                    }
                    catch { }
                }

                throw new AuthException(message);
            }

            if (string.IsNullOrWhiteSpace(body))
                return default;

            return JsonUtility.FromJson<TResponse>(body);
        }
        #endregion
    }
}
