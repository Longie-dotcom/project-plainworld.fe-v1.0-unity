using Assets.Network.DTO;
using Assets.Network.Interface.Base;

namespace Assets.Network.Interface.Receiver
{
    public interface IConsoleNetworkReceiver : INetworkBase
    {
        void OnPlayerChatted(ChatDTO dto);
        void OnPlayerEntityChatted(ChatDTO dto);
    }
}
