namespace Assets.Network
{
    public interface INetworkHandler
    {
        // Called by NetworkService when a network event is received
        void HandleNetworkEvent(string group, string method, object payload);

        // Called by the service itself to send an event through NetworkService
        void SendNetworkEvent(string group, string method, object payload);
    }
}