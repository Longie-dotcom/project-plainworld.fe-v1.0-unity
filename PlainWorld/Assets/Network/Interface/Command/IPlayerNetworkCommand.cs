using Assets.Network.DTO;
using System.Threading.Tasks;

namespace Assets.Network.Interface.Command
{
    public interface IPlayerNetworkCommand
    {
        Task Join();
        Task Logout();
        Task Act(PlayerActsDTO dto);
        Task CreateAppearance(PlayerCreateAppearanceDTO dto);
    }
}
