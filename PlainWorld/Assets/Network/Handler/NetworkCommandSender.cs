using Assets.Utility;
using System;
using System.Threading.Tasks;

namespace Assets.Network.Handler
{
    public class NetworkCommandSender
    {
        #region Attributes
        private NetworkService network;
        #endregion

        #region Properties
        #endregion

        public NetworkCommandSender() { }

        #region Methods
        public void BindNetwork(NetworkService network)
        {
            this.network = network;
        }

        public async Task Send(
            string method,
            params object[] args)
        {
            int retryCount = 3;
            int delay = 200;

            for (int i = 0; i < retryCount; i++)
            {
                try
                {
                    await network.SendEvent(method, args);
                    return;
                }
                catch (Exception ex)
                {
                    if (i == retryCount - 1)
                    {
                        GameLogger.Error(
                            Channel.Network,
                            $"Send failed after {retryCount} retries: {ex.Message}");
                        return;
                    }

                    await Task.Delay(delay);
                    delay *= 2;
                }
            }
        }
        #endregion
    }
}
