using Assets.Service;
using Assets.Network;
using Assets.Utility;
using UnityEngine;

namespace Assets.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        #region Attributes
        #endregion

        #region Properties
        #endregion

        private void Awake()
        {
            GameLogger.Info(Channel.System, "Game services are initializing");

            // Make sure coroutine runner exists
            var runner = CoroutineRunner.Instance;

            // --- Instantiate services ---
            var networkService = new NetworkService();
            var playerService = new PlayerService();

            // --- Register services ---
            ServiceLocator.Register<NetworkService>(networkService);
            ServiceLocator.Register<PlayerService>(playerService);

            // --- Initialize services ---
            networkService.Initialize();
            playerService.Initialize();

            GameLogger.Info(Channel.System, "All have been initialized successfully");
        }

        private void OnApplicationQuit()
        {
            ServiceLocator.ShutdownAll();
        }
    }
}