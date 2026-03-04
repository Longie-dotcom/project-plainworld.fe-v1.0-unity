using Assets.Core;
using Assets.Data.Enum;
using Assets.Network.DTO;
using Assets.Network.Interface.Command;
using Assets.Service.Interface;
using Assets.State;
using Assets.State.Component.Player;
using Assets.State.Interface.State;
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

        public void UnloadPlayerData()
        {
            playerState.UnloadPlayerData();
        }

        #region Movement
        public void ApplyPredictedAction(Vector2 dir, EntityAction action)
        {
            playerState.ApplyPredictedAction(dir, action);
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
        #endregion

        #region Inventory
        public void SwapInventory(int from, int to)
        {
            playerState.SwapInventory(from, to);
        }

        public void SetInventoryItem(int index, InventoryItemSnapshot item)
        {
            playerState.SetInventoryItem(index, item);
        }

        public void SelectInventorySlot(int index)
        {
            playerState.SelectInventorySlot(index);
        }

        public void RemoveSelectedItem(int quantity = 1)
        {
            playerState.RemoveSelectedItem(quantity);
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

        public async Task ActAsync()
        {
            if (!playerState.TryCreateActionCreation(out var snapshot))
                return;

            var dto = new PlayerActsDTO
            {
                Direction = PositionMapper.ToDTO(snapshot.direction),
                Action = snapshot.action,
                DeltaTime = Time.deltaTime,
            };
            await PlayerNetworkCommand.Act(dto);
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

        public async Task PlaceWorldObject(Vector2 position, string itemId)
        {
            if (!PlayerState.HasJoined)
                return;

            var dto = new PlayerPlaceWorldObjectDTO
            {
                ItemID = itemId,
                Position = PositionMapper.ToDTO(position),
            };
            await PlayerNetworkCommand.PlaceWorldObject(dto);
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
                    ActMapper.ToSnapshot(dto.Act),
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

        public void OnPlayerActed(PlayerActDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                playerState.ApplyServerAction(
                    dto.ID,
                    ActMapper.ToSnapshot(dto.Act))
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

        public void OnPlayerPickedItem(Item item)
        {
            CoroutineRunner.Instance.Schedule(() =>
                playerState.PickUpItem(
                    ItemMapper.ToSnapshot(item))
            );
        }

        public void OnWorldObjectPlaced(WorldObjectDTO dto)
        {
            CoroutineRunner.Instance.Schedule(() =>
                playerState.OnWorldObjectPlace(
                    PositionMapper.ToVector2(dto.Position),
                    dto.ItemID)
            );
        }
        #endregion
        #endregion
    }
}
