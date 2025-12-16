using System.Threading.Tasks;

namespace Assets.Core
{
    public interface IService
    {
        bool IsInitialized { get; }
        Task InitializeAsync();
        Task ShutdownAsync();
    }
}