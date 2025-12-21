using Assets.Network.DTO;
using System.Threading.Tasks;

namespace Assets.Network.Interface.Command
{
    public interface IPlayerNetworkCommand
    {
        Task Join(PlayerJoinDTO dto);
        Task Move(PlayerMoveDTO dto);
        Task Logout(PlayerLogoutDTO dto);
    }
}
