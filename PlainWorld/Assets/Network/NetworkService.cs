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
        private bool isConnected = false;
        private const string HUB_URL = "http://26.92.115.30:5020/hubs/game";
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        #endregion

        public NetworkService() { }

        #region Methods
        public async Task InitializeAsync()
        {
            GameLogger.Info(Channel.Network, "The network is initializing");

            connection = new HubConnectionBuilder()
                .WithUrl(HUB_URL)
                .WithAutomaticReconnect()
                .Build();

            BindServerEvents();

            try
            {
                await connection.StartAsync();
                isConnected = true;

                GameLogger.Info(
                    Channel.Network, "The network connected successfully");
            }
            catch (Exception ex)
            {
                GameLogger.Error(
                    Channel.Network, $"The network connection has an exception: {ex.Message}");
            }

            IsInitialized = true;
        }

        public async Task ShutdownAsync()
        {
            if (connection != null)
            {
                GameLogger.Warning(
                    Channel.Network, "The network is shut down");

                await connection.StopAsync();
                await connection.DisposeAsync();
                isConnected = false;
            }
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

        public async Task JoinGroup(string groupName)
        {
            if (connection == null || connection.State != HubConnectionState.Connected)
                throw new Exception("Network is not ready");

            try
            {
                await connection.InvokeAsync(OnSend.JoinGroup, groupName);
                GameLogger.Info(
                    Channel.Network, $"Joined group {groupName}");
            }
            catch (Exception ex)
            {
                GameLogger.Error(
                    Channel.Network, $"Failed to join group {groupName}: {ex.Message}");
            }
        }
        #endregion

        #region Private Helpers
        private void BindServerEvents()
        {
            // --- Player Service ---
            connection.On<Guid, PositionDTO>(
                OnReceive.OnPlayerJoin, (playerId, position) =>
                {
                    var dto = new PlayerJoinDTO { PlayerId = playerId, Position = position };
                    Dispatch<IPlayerNetworkReceiver, PlayerJoinDTO>(
                        dto, (r, d) => r.OnPlayerJoined(d));
                });

            connection.On<Guid, PositionDTO>(
                OnReceive.OnPlayerMove, (playerId, position) =>
                {
                    var dto = new PlayerMoveDTO { PlayerId = playerId, Position = position };
                    Dispatch<IPlayerNetworkReceiver, PlayerMoveDTO>(
                        dto, (r, d) => r.OnPlayerMoved(d));
                });

            // --- Entity Service ---
            connection.On<Guid, PositionDTO>(
                OnReceive.OnPlayerEntityJoin, (playerId, position) =>
                {
                    var dto = new PlayerJoinDTO { PlayerId = playerId, Position = position };
                    Dispatch<IEntityNetworkReceiver, PlayerJoinDTO>(
                        dto, (r, d) => r.OnPlayerEntityJoined(d));
                });

            connection.On<Guid, PositionDTO>(
                OnReceive.OnPlayerEntityMove, (playerId, position) =>
                {
                    var dto = new PlayerMoveDTO { PlayerId = playerId, Position = position };
                    Dispatch<IEntityNetworkReceiver, PlayerMoveDTO>(
                        dto, (r, d) => r.OnPlayerEntityMoved(d));
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