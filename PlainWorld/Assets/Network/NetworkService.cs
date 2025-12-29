using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Base;
using Assets.Network.Interface.Receiver;
using Assets.Utility;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assets.Network
{
    public class NetworkService : IService
    {
        #region Attributes
        private readonly Dictionary<Type, object> receivers = new();
        private readonly Dictionary<Type, Queue<Action>> pendingEvents = new();

        private HubConnection connection;
        private const string HUB_URL = "http://26.92.115.30:5020/hubs/game"; // 192.168.1.135:5020
        #endregion

        #region Properties
        public bool IsConnected { get; private set; } = false;
        public bool IsInitialized { get; private set; } = false;
        #endregion

        public NetworkService() { }

        #region Methods
        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public async Task ShutdownAsync()
        {
            if (connection != null)
            {
                GameLogger.Warning(
                    Channel.Network, "The network is shut down");

                await connection.StopAsync();
                await connection.DisposeAsync();
                IsConnected = false;
            }
        }

        public async Task ConnectAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("AccessToken missing");

            connection = new HubConnectionBuilder()
                .WithUrl(HUB_URL, options =>
                {
                    options.AccessTokenProvider =
                        () => Task.FromResult(accessToken);
                })
                .WithAutomaticReconnect()
                .Build();

            BindServerEvents();

            await connection.StartAsync();
            IsConnected = true;
        }

        public void Register<T>(T receiver) where T : class
        {
            var type = typeof(T);
            receivers[type] = receiver;

            if (pendingEvents.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                    queue.Dequeue().Invoke();
                pendingEvents.Remove(type);
            }
        }

        public void Unregister<T>()
        {
            receivers.Remove(typeof(T));
        }

        public async Task SendEvent(
            string method,
            params object[] args)
        {
            if (connection == null || connection.State != HubConnectionState.Connected)
                throw new Exception("Network is not ready");

            try
            {
                await connection.InvokeCoreAsync(method, args);
                GameLogger.Info(
                    Channel.Network, $"Send event with method {method}");
            }
            catch (Exception ex)
            {
                GameLogger.Error(
                    Channel.Network, $"Send event failed: {ex.Message}");
            }
        }
        #endregion

        #region Private Helpers
        private void BindServerEvents()
        {
            // --- Player Service ---
            connection.On<PlayerDTO>(
                OnReceive.OnPlayerJoin, dto =>
                {
                    Dispatch<IPlayerNetworkReceiver, PlayerDTO>(
                        dto, (r, d) => r.OnPlayerJoined(d));
                });

            connection.On<Guid>(
                OnReceive.OnPlayerLogout, id =>
                {
                    Dispatch<IPlayerNetworkReceiver, Guid>(
                        id, (r, d) => r.OnPlayerLogout(d));
                });

            connection.On<PlayerMovementDTO>(
                OnReceive.OnPlayerMove, dto =>
                {
                    Dispatch<IPlayerNetworkReceiver, PlayerMovementDTO>(
                        dto, (r, d) => r.OnPlayerMoved(d));
                });

            connection.On<PlayerAppearanceDTO>(
                OnReceive.OnPlayerCreateAppearance, dto =>
                {
                    Dispatch<IPlayerNetworkReceiver, PlayerAppearanceDTO>(
                        dto, (r, d) => r.OnPlayerCreatedAppearance(d));
                });

            // --- Entity Service ---
            connection.On<PlayerEntityDTO>(
                OnReceive.OnPlayerEntityJoin, dto =>
                {
                    Dispatch<IEntityNetworkReceiver, PlayerEntityDTO>(
                        dto, (r, d) => r.OnPlayerEntityJoined(d));
                });

            connection.On<Guid>(
                OnReceive.OnPlayerEntityLogout, id =>
                {
                    Dispatch<IEntityNetworkReceiver, Guid>(
                        id, (r, d) => r.OnPlayerEntityLogout(d));
                });

            connection.On<PlayerEntityMovementDTO>(
                OnReceive.OnPlayerEntityMove, dto =>
                {
                    Dispatch<IEntityNetworkReceiver, PlayerEntityMovementDTO>(
                        dto, (r, d) => r.OnPlayerEntityMoved(d));
                });

            connection.On<PlayerEntityAppearanceDTO>(
                OnReceive.OnPlayerEntityCreateAppearance, dto =>
                {
                    Dispatch<IEntityNetworkReceiver, PlayerEntityAppearanceDTO>(
                        dto, (r, d) => r.OnPlayerEntityCreatedAppearance(d));
                });

            connection.On<IEnumerable<PlayerEntityDTO>>(
                OnReceive.OnPlayerEntityOnline, dtos =>
                {
                    foreach (var dto in dtos)
                    {
                        Dispatch<IEntityNetworkReceiver, PlayerEntityDTO>(
                            dto, (r, d) => r.OnPlayerEntityJoined(d));
                    }
                });
        }

        private void Dispatch<TReceiver, TData>(
            TData data,
            Action<TReceiver, TData> call,
            string group = null)
            where TReceiver : class
        {
            bool calledAny = false;

            foreach (var receiver in receivers.Values)
            {
                // Call methods
                if (receiver is TReceiver r)
                {
                    if (group == null || (r is INetworkBase gr && gr.Group == group))
                    {
                        call(r, data);
                        calledAny = true;
                    }
                }
            }

            if (!calledAny)
            {
                // Fallback for pending events
                var type = typeof(TReceiver);
                if (!pendingEvents.TryGetValue(type, out var queue))
                    queue = new Queue<Action>();

                queue.Enqueue(() => {
                    if (receivers.TryGetValue(type, out var later))
                        call((TReceiver)later, data);
                    else
                        GameLogger.Warning(
                            Channel.Network, $"Pending event for {type} dropped, receiver still not registered");
                });

                pendingEvents[type] = queue;
            }
        }
        #endregion
    }
}