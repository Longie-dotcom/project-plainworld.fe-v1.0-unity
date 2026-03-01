using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Service.Interface;
using Assets.State;
using Assets.State.Interface.State;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class ConsoleService : IService
    {
        #region Attributes
        private readonly ConsoleState consoleState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IConsoleNetworkCommand ConsoleNetworkCommand { get; private set; }
        public IReadOnlyConsoleState ConsoleState { get { return consoleState; } }
        #endregion

        public ConsoleService()
        {
            consoleState = new ConsoleState();
        }

        #region Methods
        public Task InitializeAsync()
        {
            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IConsoleNetworkCommand command)
        {
            ConsoleNetworkCommand = command;
        }

        #region Senders
        public async Task PlayerChat(string message)
        {
            if (!ServiceLocator.Get<PlayerService>().PlayerState.HasJoined)
                return;

            var dto = new ChatSendDTO()
            {
                Content = message
            };
            await ConsoleNetworkCommand.Chat(dto);
        }
        #endregion

        #region Receivers
        public void OnPlayerChatted(ChatDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
            {
                consoleState.OnPlayerChatted(
                    dto.UserName,
                    dto.Content);
            });
        }

        public void OnPlayerEntityChatted(ChatDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
            {
                consoleState.OnPlayerEntityChatted(
                    dto.UserName,
                    dto.Content);
            });
        }
        #endregion
        #endregion
    }
}
