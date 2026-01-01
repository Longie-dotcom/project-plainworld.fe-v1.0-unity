using System.Threading.Tasks;

namespace Assets.Service.Interface
{
    public interface IService
    {
        bool IsInitialized { get; }
        Task InitializeAsync();
        Task ShutdownAsync();
    }
}