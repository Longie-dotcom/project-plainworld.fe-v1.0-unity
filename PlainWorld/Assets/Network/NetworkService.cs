using Assets.Core;
using Assets.Utility;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;

namespace Assets.Network
{
    public class NetworkService : IService
    {
        #region Attributes
        private readonly List<INetworkHandler> handlers = new();

        private HubConnection connection;
        private bool isConnected = false;
        private const string HUB_URL = "https://your-server-url/hub";
        #endregion

        #region Properties
        public event Action<string, string, object> OnRawNetworkEvent;
        #endregion

        public NetworkService() { }

        #region Methods
        public async void Initialize()
        {
            GameLogger.Info(Channel.Network, "The network is initializing");

            connection = new HubConnectionBuilder()
                .WithUrl(HUB_URL)
                .WithAutomaticReconnect()
                .Build();

            connection.On<string, string, object>("OnNetworkEvent",
                (group, method, payload) =>
                {
                    foreach (var handler in handlers)
                    {
                        handler.HandleNetworkEvent(group, method, payload);
                    }
                });

            try
            {
                await connection.StartAsync();
                isConnected = true;

                GameLogger.Info(Channel.Network, "The network connected successfully");
            }
            catch (Exception ex)
            {
                GameLogger.Error(Channel.Network, $"The network connection has an exception: {ex.Message}");
            }
        }

        public async void Shutdown()
        {
            if (connection != null)
            {
                GameLogger.Warning(Channel.Network, "The network is shut down");

                await connection.StopAsync();
                await connection.DisposeAsync();
                isConnected = false;
            }
        }

        public void RegisterHandler(INetworkHandler handler)
        {
            if (!handlers.Contains(handler))
                handlers.Add(handler);
        }

        public void UnregisterHandler(INetworkHandler handler)
        {
            if (handlers.Contains(handler))
                handlers.Remove(handler);
        }

        public async void SendEvent(
            string group, 
            string method, 
            object payload)
        {
            if (connection == null || connection.State != HubConnectionState.Connected) return;

            try
            {
                GameLogger.Info(Channel.Network, $"Send event with method {method} for group: {group}");
                await connection.InvokeAsync("SendEvent", group, method, payload);
            }
            catch (Exception ex)
            {
                GameLogger.Error(Channel.Network, $"Send event failed: {ex.Message}");
            }
        }
        #endregion
    }
}