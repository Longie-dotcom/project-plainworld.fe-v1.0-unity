using Assets.Network.Interface.Command;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using Assets.Utility;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Network.DTO;

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
        private HttpClient httpClient;
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

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(BASE_URL),
                Timeout = TimeSpan.FromSeconds(10)
            };
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
            var payload = new RegisterRequest
            {
                email = email,
                password = password,
                fullName = fullName,
                gender = gender,
                dob = DateTime.Parse(dob).ToString("o")
            };

            await PostAsync<RegisterRequest, object>(
                "iam/auth/register",
                payload);
        }
        #endregion

        #region Private Helpers
        private async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint,
            TRequest payload)
        {
            var json = JsonUtility.ToJson(payload);
            GameLogger.Info(Channel.System, $"[AUTH][REQUEST] {endpoint}\n{json}");

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(endpoint, content);
            var body = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                GameLogger.Error(
                    Channel.System,
                    $"[AUTH][ERROR] {endpoint}\n" +
                    $"Status: {(int)response.StatusCode} {response.ReasonPhrase}\n" +
                    $"Body:\n{body}");

                throw new Exception($"Request failed: {endpoint}");
            }

            if (string.IsNullOrWhiteSpace(body))
                return default;

            return JsonUtility.FromJson<TResponse>(body);
        }
        #endregion
    }
}
