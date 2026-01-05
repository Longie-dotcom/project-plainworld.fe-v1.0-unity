using Assets.Core;
using Assets.Network.Handler;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using Assets.Utility;
using System.Threading.Tasks;

namespace Assets.Network
{
    public sealed class NetworkSessionCoordinator
    {
        #region Attributes
        private readonly NetworkService network;

        private PlayerNetworkHandler player;
        private EntityNetworkHandler entity;
        private UINetworkHandler ui;
        private GameNetworkHandler game;
        private AuthNetworkHandler auth;
        private CursorNetworkHandler cursor;
        private SettingNetworkHandler setting;
        #endregion

        #region Properties
        #endregion

        public NetworkSessionCoordinator(NetworkService network)
        {
            this.network = network;
        }

        #region Methods
        public async Task StartSessionAsync()
        {
            // Get services (app-scoped)
            var playerService = ServiceLocator.Get<PlayerService>();
            var entityService = ServiceLocator.Get<EntityService>();
            var uiService = ServiceLocator.Get<UIService>();
            var gameService = ServiceLocator.Get<GameService>();
            var authService = ServiceLocator.Get<AuthService>();
            var cursorService = ServiceLocator.Get<CursorService>();
            var settingService = ServiceLocator.Get<SettingService>();

            // --- Instantiate handlers (session-scoped) ---
            player = new PlayerNetworkHandler();
            entity = new EntityNetworkHandler();
            ui = new UINetworkHandler();
            game = new GameNetworkHandler();
            auth = new AuthNetworkHandler();
            cursor = new CursorNetworkHandler();
            setting = new SettingNetworkHandler();

            // --- Bind services to handlers to network ---
            playerService.BindNetworkCommand(player);
            player.BindService(playerService, network);

            entityService.BindNetworkCommand(entity);
            entity.BindService(entityService, network);

            uiService.BindNetworkCommand(ui);
            ui.BindService(uiService, network);

            gameService.BindNetworkCommand(game);
            game.BindService(gameService, network);

            authService.BindNetworkCommand(auth);
            auth.BindService(authService, network);

            cursorService.BindNetworkCommand(cursor);
            cursor.BindService(cursorService, network);

            settingService.BindNetworkCommand(setting);
            setting.BindService(settingService, network);

            // --- Register receivers ---
            network.Register<IPlayerNetworkReceiver>(player);
            network.Register<IEntityNetworkReceiver>(entity);
            network.Register<IUINetworkReceiver>(ui);
            network.Register<IGameNetworkReceiver>(game);
            network.Register<IAuthNetworkReceiver>(auth);
            network.Register<ICursorNetworkReceiver>(cursor);
            network.Register<ISettingNetworkReceiver>(setting);

            // --- Ensure everything is ready ---
            await network.WaitUntilReady();

            GameLogger.Info(
                Channel.System, 
                "Network session started successfully");
        }

        public Task EndSessionAsync()
        {
            // --- Unregister all handlers ---
            network.Unregister<IPlayerNetworkReceiver>();
            network.Unregister<IEntityNetworkReceiver>();
            network.Unregister<IUINetworkReceiver>();
            network.Unregister<IGameNetworkReceiver>();
            network.Unregister<IAuthNetworkReceiver>();
            network.Unregister<ICursorNetworkReceiver>();
            network.Unregister<ISettingNetworkReceiver>();

            // --- Clear all handlers ---
            player = null;
            entity = null;
            ui = null;
            game = null;
            auth = null;
            cursor = null;
            setting = null;

            GameLogger.Info(
                Channel.System, 
                "Network session ended");

            // --- Pre-session Auth (HTTP) ---
            var authService = ServiceLocator.Get<AuthService>();
            var preAuthSession = new AuthNetworkHandler();
            authService.BindNetworkCommand(preAuthSession);

            GameLogger.Info(
                Channel.System, 
                "Re-bind http pre-session for Auth Service successfully");

            return Task.CompletedTask;
        }
        #endregion
    }
}
