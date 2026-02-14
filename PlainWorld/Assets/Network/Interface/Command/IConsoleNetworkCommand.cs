using Assets.Network.DTO;
using System.Threading.Tasks;

namespace Assets.Network.Interface.Command
{
    public interface IConsoleNetworkCommand
    {
        Task Chat(ChatSendDTO dto);
    }
}
