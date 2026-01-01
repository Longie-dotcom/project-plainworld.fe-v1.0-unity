using Assets.Core;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Service.Interface;
using Assets.State;
using Assets.State.Component.Player;
using Assets.State.Interface.IReadOnlyState;
using Assets.Utility;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Service
{
    public class PlayerService : IService
    {
        #region Attributes
        private readonly PlayerState playerState;
        #endregion

        #region Properties
        public bool IsInitialized { get; private set; } = false;
        public IPlayerNetworkCommand PlayerNetworkCommand { get; private set; }
        public IReadOnlyPlayerState PlayerState { get { return playerState; } }
        #endregion

        public PlayerService()
        {
            playerState = new PlayerState();
        }

        #region Methods
        public Task InitializeAsync()
        {
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

        #region Movement
        public void ApplyPredictedPosition(Vector2 dir)
        {
            playerState.ApplyPredictedPosition(dir);
        }
        #endregion

        #region Appearance
        public void SetHair(string id)
        {
            playerState.SetHair(id);
        }

        public void SetGlasses(string id)
        {
            playerState.SetGlasses(id);
        }

        public void SetShirt(string id)
        {
            playerState.SetShirt(id);
        }

        public void SetPant(string id)
        {
            playerState.SetPant(id);
        }

        public void SetShoe(string id)
        {
            playerState.SetShoe(id);
        }

        public void SetEyes(string id)
        {
            playerState.SetEyes(id);
        }

        public void SetSkin(string id)
        {
            playerState.SetSkin(id);
        }

        public void SetHairColor(float h, float s, float v)
        {
            playerState.SetHairColor(h, s, v);
        }

        public void SetPantColor(float h, float s, float v)
        {
            playerState.SetPantColor(h, s, v);
        }

        public void SetEyeColor(float h, float s, float v)
        {
            playerState.SetEyeColor(h, s, v);
        }

        public void SetSkinColor(float h, float s, float v)
        {
            playerState.SetSkinColor(h, s, v);
        }

        public void ApplyDefaultAppearance(
            PlayerAppearanceSnapshot snapshot, 
            PlayerAppearanceSnapshot defaults)
        {
            playerState.NormalizeAppearance(snapshot, defaults);
        }

        public void RequireCreateAppearance()
        {
            playerState.RequireCreateAppearance();
        }
        #endregion

        #region Senders
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
            if (!playerState.TryCreateMovementCreation(out var snapshot))
                return;

            var dto = new PlayerMoveDTO
            {
                Movement = PlayerMovementMapper.ToDTO(snapshot)
            };
            await PlayerNetworkCommand.Move(dto);
        }

        public async Task CreateAppearanceAsync()
        {
            if (!playerState.TryPrepareAppearanceCreation(out var snapshot))
                return;

            var dto = new PlayerCreateAppearanceDTO
            {
                Appearance = PlayerAppearanceMapper.ToDTO(snapshot)
            };
            await PlayerNetworkCommand.CreateAppearance(dto);
        }
        #endregion

        #region Receivers
        public void OnPlayerJoined(PlayerDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
            {
                playerState.LoadPlayerData(
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
                playerState.Logout(id)
            );
        }

        public void OnPlayerMoved(PlayerMovementDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                playerState.ApplyServerMovement(
                    dto.ID,
                    PlayerMovementMapper.ToSnapshot(dto.Movement))
            );
        }

        public void OnPlayerCreatedAppearance(PlayerAppearanceDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                playerState.ApplyServerAppearance(
                    dto.ID,
                    PlayerAppearanceMapper.ToSnapshot(dto.Appearance))
            );
        }

        public void OnPlayerForcedLogout()
        {
            CoroutineRunner.Instance.Schedule(() => 
                playerState.ForcedLogout()
            );
        }
        #endregion
        #endregion
    }
}
