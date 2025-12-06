using Assets.Core;
using Assets.Network;
using Assets.Utility;

namespace Assets.Service
{
    public class PlayerService : IService, INetworkHandler
    {
        #region Attributes
        private NetworkService networkService;
        #endregion

        #region Properties
        #endregion

        public PlayerService() { }

        #region Methods
        public void Initialize()
        {
            networkService = ServiceLocator.Get<NetworkService>();
            networkService.RegisterHandler(this);
        }

        public void Shutdown()
        {
            networkService.UnregisterHandler(this);
        }

        public void HandleNetworkEvent(string group, string method, object payload)
        {
            switch (method)
            {
                case NetworkMethod.PlayerJoined:
                    SpawnPlayer(payload);
                    break;
                case NetworkMethod.PlayerMoved:
                    UpdateMovement(payload);
                    break;
            }
        }

        public void SendNetworkEvent(string group, string method, object payload)
        {
            networkService.SendEvent(group, method, payload);
        }

        #region Handler received event
        private void SpawnPlayer(object payload)
        {
            GameLogger.Info(Channel.Service, $"Spwaned new player");
        }

        private void UpdateMovement(object payload)
        {
            GameLogger.Info(Channel.Service, $"Update player movement");
        }
        #endregion
        #endregion
    }
}
