using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Network.DTO;
using Assets.Network.Interface.Base;
using Assets.Network.Interface.Receiver;
using Assets.Service.Interface;
using Assets.Utility;
using Microsoft.AspNetCore.SignalR.Client;

namespace Assets.Network
{
    public class NetworkService : IService
    {
        #region Attributes
        private readonly Dictionary<Type, object> handlers = new();
        private readonly Dictionary<Type, Queue<Action>> pendingHandlers = new();

        private HubConnection connection;
        private const string HUB_URL = "http://171.239.243.177:5020/hubs/game"; // 192.168.1.135:5020
        #endregion

        #region Properties
        public NetworkSessionCoordinator Session { get; private set; }

        public bool IsReady
        {
            get { return connection != null && connection.State == HubConnectionState.Connected; }
        }
        public bool IsBinded { get; private set; } = false;
        public bool IsConnected { get; private set; } = false;
        public bool IsInitialized { get; private set; } = false;
        #endregion

        public NetworkService() { }

        #region Methods
        public Task InitializeAsync()
        {
            Session = new NetworkSessionCoordinator(this);
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public async Task ShutdownAsync()
        {
            handlers.Clear();
            pendingHandlers.Clear();

            if (connection != null)
            {
                await connection.StopAsync();
                await connection.DisposeAsync();
                connection = null;
            }

            IsConnected = false;
            IsBinded = false;

            GameLogger.Warning(Channel.Network, "Network shut down");
        }

        public async Task ConnectAsync(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("AccessToken missing");

            // Shut down the current to start new cycle
            await ShutdownAsync();

            connection = new HubConnectionBuilder()
                .WithUrl(HUB_URL, options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .WithAutomaticReconnect()
                .Build();

            BindServerEvents();

            connection.Reconnected += id =>
            {
                IsConnected = true;
                GameLogger.Info(Channel.Network, "Reconnected");
                return Task.CompletedTask;
            };

            connection.Reconnecting += ex =>
            {
                IsConnected = false;
                GameLogger.Warning(Channel.Network, "Reconnecting...");
                return Task.CompletedTask;
            };

            connection.Closed += ex =>
            {
                IsConnected = false;
                GameLogger.Warning(Channel.Network, "Disconnected");
                return Task.CompletedTask;
            };

            await connection.StartAsync();
            IsConnected = true;

            GameLogger.Info(Channel.Network, "Connected");
        }

        public async Task WaitUntilReady()
        {
            while (!IsReady)
            {
                if (connection == null)
                    throw new OperationCanceledException("Network shut down");

                await Task.Delay(50);
            }
        }

        public async Task SendEvent(
            string method,
            params object[] args)
        {
            await WaitUntilReady();

            await connection.InvokeCoreAsync(method, args);

            GameLogger.Info(Channel.Network, $"Sent event method: {method}");
        }

        #region Handlers Registration
        public void Register<T>(T handler) where T : class
        {
            var type = typeof(T);
            handlers[type] = handler;

            if (pendingHandlers.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                    queue.Dequeue().Invoke();
                pendingHandlers.Remove(type);
            }
        }

        public void Unregister<T>()
        {
            handlers.Remove(typeof(T));
        }
        #endregion
        #endregion

        #region Private Helpers
        private void BindServerEvents()
        {
            if (IsBinded) return;

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

            connection.On<PlayerActDTO>(
                OnReceive.OnPlayerAct, dto =>
                {
                    Dispatch<IPlayerNetworkReceiver, PlayerActDTO>(
                        dto, (r, d) => r.OnPlayerActed(d));
                });

            connection.On<PlayerAppearanceDTO>(
                OnReceive.OnPlayerCreateAppearance, dto =>
                {
                    Dispatch<IPlayerNetworkReceiver, PlayerAppearanceDTO>(
                        dto, (r, d) => r.OnPlayerCreatedAppearance(d));
                });

            connection.On(
                OnReceive.OnPlayerForcedLogout, () =>
                {
                    Dispatch<IPlayerNetworkReceiver, object>(
                         null, (r, _) => r.OnPlayerForcedLogout());
                });

            connection.On<Item>(
                OnReceive.OnPlayerPickItem, dto =>
                {
                    Dispatch<IPlayerNetworkReceiver, Item>(
                         dto, (r, _) => r.OnPlayerPickedItem(dto));
                });

            connection.On<WorldObjectDTO>(
                OnReceive.OnWorldObjectPlaced, dto =>
                {
                    Dispatch<IPlayerNetworkReceiver, WorldObjectDTO>(
                         dto, (r, _) => r.OnWorldObjectPlaced(dto));
                });

            connection.On<IEnumerable<WorldObjectDTO>>(
                OnReceive.OnWorldObjectPlaced, dtos =>
                {
                    foreach (var dto in dtos)
                    {
                        Dispatch<IPlayerNetworkReceiver, WorldObjectDTO>(
                            dto, (r, d) => r.OnWorldObjectPlaced(d));
                    }
                });

            connection.On<ChatDTO>(
                OnReceive.OnPlayerChat, dto =>
                {
                    Dispatch<IConsoleNetworkReceiver, ChatDTO>(
                         dto, (r, d) => r.OnPlayerChatted(d));
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

            connection.On<PlayerEntityActDTO>(
                OnReceive.OnPlayerEntityAct, dto =>
                {
                    Dispatch<IEntityNetworkReceiver, PlayerEntityActDTO>(
                        dto, (r, d) => r.OnPlayerEntityActed(d));
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

            connection.On<GrayShroomEntityDTO>(
                OnReceive.OnGrayShroomEntitySpawn, dto =>
                {
                    Dispatch<IEntityNetworkReceiver, GrayShroomEntityDTO>(
                        dto, (r, d) => r.OnGrayShroomEntitySpawned(d));
                });

            connection.On<GrayShroomEntityActDTO>(
                OnReceive.OnGrayShroomEntityAct, dto =>
                {
                    Dispatch<IEntityNetworkReceiver, GrayShroomEntityActDTO>(
                        dto, (r, d) => r.OnGrayShroomEntityActed(d));
                });

            connection.On<Guid>(
                OnReceive.OnGrayShroomEntityDespawn, id =>
                {
                    Dispatch<IEntityNetworkReceiver, Guid>(
                        id, (r, d) => r.OnGrayShroomEntityDespawned(d));
                });

            connection.On<IEnumerable<GrayShroomEntityDTO>>(
                OnReceive.OnGrayShroomEntityOnline, dtos =>
                {
                    foreach (var dto in dtos)
                    {
                        Dispatch<IEntityNetworkReceiver, GrayShroomEntityDTO>(
                            dto, (r, d) => r.OnGrayShroomEntitySpawned(d));
                    }
                });

            // --- Console Service ---
            connection.On<ChatDTO>(
                OnReceive.OnPlayerEntityChat, dto =>
                {
                    Dispatch<IConsoleNetworkReceiver, ChatDTO>(
                         dto, (r, d) => r.OnPlayerEntityChatted(d));
                });

            IsBinded = true;

            GameLogger.Info(Channel.Network, "Binded network event handlers successfully");
        }

        private void Dispatch<TReceiver, TData>(
            TData data,
            Action<TReceiver, TData> call,
            string group = null)
            where TReceiver : class
        {
            bool calledAny = false;

            foreach (var receiver in handlers.Values)
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
                if (!pendingHandlers.TryGetValue(type, out var queue))
                    queue = new Queue<Action>();

                queue.Enqueue(() =>
                {
                    if (handlers.TryGetValue(type, out var later))
                        call((TReceiver)later, data);
                    else
                        GameLogger.Warning(
                            Channel.Network, $"Pending event for {type} dropped, receiver still not registered");
                });

                pendingHandlers[type] = queue;
            }
        }
        #endregion
    }
}