using Assets.Network.DTO;
using System.Threading.Tasks;

namespace Assets.Network.Interface.Command
{
    public interface IAuthNetworkCommand
    {
        Task<HttpResponse<TokenPayload>> Login(
            string email, 
            string password);
        Task Register(
            string email,
            string password,
            string fullName,
            string gender,
            string dob);
    }
}
