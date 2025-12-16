using Assets.Network;
using Assets.Network.Handler;
using Assets.Network.Interface.Receiver;
using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

namespace Assets.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion


        #region Methods
        private void Awake()
        {
            GameLogger.Info(
                Channel.System, 
                "Game services are initializing");

            // Make sure coroutine runner exists
            var runner = CoroutineRunner.Instance;

            // --- Instantiate services ---
            var networkService = new NetworkService();
            var stateService = new StateService();
            var playerService = new PlayerService();
            var entityService = new EntityService();
            var uiService = new UIService();
            var authService = new AuthService();

            GameLogger.Info(
                Channel.System,
                "Instantiate services successfully");

            // --- Instantiate network handlers ---
            var playerNetworkHandler = new PlayerNetworkHandler();
            var entityNetworkHandler = new EntityNetworkHandler();
            var uiNetworkHandler = new UINetworkHandler();
            var stateNetworkHandler = new StateNetworkHandler();

            GameLogger.Info(
                Channel.System,
                "Instantiate network handlers successfully");

            // --- Bind dependencies ---
            stateService.BindNetworkCommand(stateNetworkHandler);
            stateNetworkHandler.BindService(stateService, networkService);
            playerService.BindNetworkCommand(playerNetworkHandler);
            playerNetworkHandler.BindService(playerService, networkService);
            entityService.BindNetworkCommand(entityNetworkHandler);
            entityNetworkHandler.BindService(entityService, networkService);
            uiService.BindNetworkCommand(uiNetworkHandler);
            uiNetworkHandler.BindService(uiService, networkService);

            GameLogger.Info(
                Channel.System,
                "Bind network handlers and services successfully");

            // --- Register receivers ---
            networkService.Register<IStateNetworkReceiver>(stateNetworkHandler);
            networkService.Register<IPlayerNetworkReceiver>(playerNetworkHandler);
            networkService.Register<IEntityNetworkReceiver>(entityNetworkHandler);
            networkService.Register<IUINetworkReceiver>(uiNetworkHandler);

            GameLogger.Info(
                Channel.System,
                "Register network hanlders successfully");

            // --- Register services --- 
            ServiceLocator.Register<NetworkService>(networkService);
            ServiceLocator.Register<StateService>(stateService);
            ServiceLocator.Register<PlayerService>(playerService);
            ServiceLocator.Register<EntityService>(entityService);
            ServiceLocator.Register<UIService>(uiService);
            ServiceLocator.Register<AuthService>(authService);

            GameLogger.Info(
                Channel.System,
                "Register services to service locator successfully");

            // --- Initialize services ---
            runner.StartCoroutine(
                InitializeServices(
                    networkService,
                    stateService,
                    playerService,
                    entityService,
                    uiService,
                    authService));
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.ShutdownAll().Wait(1000);
        }
        #endregion

        #region Private Helpers
        private IEnumerator InitializeServices(
            NetworkService networkService,
            StateService stateService,
            PlayerService playerService,
            EntityService entityService,
            UIService uiService,
            AuthService authService)
        {
            // Network is ready first
            yield return networkService.InitializeAsync().AsCoroutine();

            // Other services ready after connection established
            yield return stateService.InitializeAsync().AsCoroutine();
            yield return playerService.InitializeAsync().AsCoroutine();
            yield return entityService.InitializeAsync().AsCoroutine();
            yield return uiService.InitializeAsync().AsCoroutine();
            yield return authService.InitializeAsync().AsCoroutine();

            GameLogger.Info(
                Channel.System, 
                "All services initialized successfully");
        }
        #endregion
    }
}