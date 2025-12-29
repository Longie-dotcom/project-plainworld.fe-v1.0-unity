using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.State.Player;
using Assets.Utility;
using System;
using System.Threading.Tasks;

namespace Assets.Service
{
    public class PlayerService : IService
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IPlayerNetworkCommand PlayerNetworkCommand { get; private set; }
        public PlayerState PlayerState { get; private set; } = new PlayerState();
        #endregion

        public PlayerService() { }

        #region Methods
        public Task InitializeAsync()
        {
            if (PlayerNetworkCommand == null)
                throw new InvalidOperationException(
                    "PlayerNetworkCommand not bound before Initialize");

            ServiceLocator.OnExiting += LogoutAsync;

            IsInitialized = true;
            return Task.CompletedTask;
        }

        public Task ShutdownAsync()
        {
            return Task.CompletedTask;
        }

        public void BindNetworkCommand(IPlayerNetworkCommand command)
        {
            PlayerNetworkCommand = command;
        }

        // Senders
        public async Task JoinAsync()
        {
            if (PlayerState.HasJoined)
                return;

            await PlayerNetworkCommand.Join();
        }

        public async Task LogoutAsync()
        {
            if (!PlayerState.HasJoined)
                return;

            await PlayerNetworkCommand.Logout();
        }

        public async Task MoveAsync()
        {
            if (!PlayerState.TryCreateMovementCreation(out var snapshot))
                return;

            var dto = new PlayerMoveDTO
            {
                Movement = PlayerMovementMapper.ToDTO(snapshot)
            };
            await PlayerNetworkCommand.Move(dto);
        }

        public async Task CreateAppearanceAsync()
        {
            if (!PlayerState.TryPrepareAppearanceCreation(out var snapshot))
                return;

            var dto = new PlayerCreateAppearanceDTO
            {
                Appearance = PlayerAppearanceMapper.ToDTO(snapshot)
            };
            await PlayerNetworkCommand.CreateAppearance(dto);
        }

        // Receivers
        public void OnPlayerJoined(PlayerDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
            {
                PlayerState.LoadPlayerData(
                    dto.ID,
                    dto.FullName,
                    PlayerMovementMapper.ToSnapshot(dto.Movement),
                    PlayerAppearanceMapper.ToSnapshot(dto.Appearance)
                );
            });
        }

        public void OnPlayerLogout(Guid id)
        {
            CoroutineRunner.Instance.Schedule(() =>
                GameLogger.Info(
                    Channel.Service,
                    $"Player with ID: {id} has logout")
            );
        }

        public void OnPlayerMoved(PlayerMovementDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                PlayerState.ApplyServerPosition(
                    dto.ID,
                    PlayerMovementMapper.ToSnapshot(dto.Movement))
            );
        }

        public void OnPlayerCreatedAppearance(PlayerAppearanceDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                PlayerState.ConfirmAppearanceCreated()
            );
        }
        #endregion
    }
}
