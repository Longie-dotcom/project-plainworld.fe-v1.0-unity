using Assets.Data.Enum;
using Assets.State.Component.Player;
using Assets.State.Component.Shared;
using Assets.State.Interface.Component.Player;
using Assets.State.Interface.Component.Shared;
using Assets.State.Interface.State;
using System;
using UnityEngine;

namespace Assets.State
{
    public class PlayerState : IReadOnlyPlayerState
    {
        #region Attributes
        private Act act;
        private PlayerAppearance appearance;
        private Inventory inventory;
        #endregion

        #region Properties
        public Guid PlayerID { get; private set; }
        public string PlayerName { get; private set; }
        public bool HasJoined { get; private set; }

        public IReadOnlyAct Act { get { return act; } }
        public IReadOnlyPlayerAppearance Appearance { get { return appearance; } }
        public IReadOnlyInventory Inventory { get { return inventory; } }

        public event Action OnPlayerDataReady;
        public event Action OnPlayerLogout;
        public event Action OnPlayerForcedLogout;


        // IMPORTANT!!!!! (REMOVE LATER, SPLIT TO ANOTHER SERVICE)
        public event Action<Vector2, string> OnWorldObjectPlaced;
        #endregion

        public PlayerState()
        {
            act = new Act();
            appearance = new PlayerAppearance();
            inventory = new Inventory(40);
        }

        #region Methods
        public void LoadPlayerData(
            Guid playerId,
            string playerName,
            ActSnapshot act,
            PlayerAppearanceSnapshot appearance)
        {
            if (HasJoined) return;

            // Load data
            PlayerID = playerId;
            PlayerName = playerName;
            HasJoined = true;
            this.appearance.ApplySnapshot(appearance);
            this.act.ApplySnapshot(act);

            // Load scene
            OnPlayerDataReady?.Invoke();
        }

        public void UnloadPlayerData()
        {
            if (!HasJoined) return;

            // Unload data
            PlayerID = Guid.Empty;
            PlayerName = null;
            HasJoined = false;
            act = new Act();
            appearance = new PlayerAppearance();
        }


        public void Logout(Guid playerId)
        {
            if (!HasJoined || playerId != PlayerID) return;
            OnPlayerLogout?.Invoke();
        }

        public void ForcedLogout()
        {
            if (!HasJoined) return;
            OnPlayerForcedLogout?.Invoke();
        }
        #endregion

        #region Action
        public bool TryCreateActionCreation(
            out (Vector2 direction, int action) snapshot)
        {
            snapshot = default;

            if (!HasJoined)
                return false;

            snapshot = act.TryCreateActionCreation();
            return true;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            if (!HasJoined) return;
            act.SetMoveSpeed(moveSpeed);
        }

        public void ApplyPredictedAction(
            Vector2 inputDir, EntityAction action)
        {
            if (!HasJoined) return;
            act.ApplyPredictedAction(inputDir, action);
        }

        public void ApplyServerAction
            (Guid id, ActSnapshot snapshot)
        {
            if (!HasJoined || id != PlayerID) return;
            act.ApplySnapshot(snapshot);
        }
        #endregion

        #region Appearance
        public bool TryPrepareAppearanceCreation(
            out PlayerAppearanceSnapshot snapshot)
        {
            snapshot = default;

            if (!HasJoined)
                return false;

            snapshot = appearance.PrepareForCreation();
            return true;
        }

        public void SetHair(string id)
        {
            if (!HasJoined) return;
            appearance.SetHair(id);
        }

        public void SetGlasses(string id)
        {
            if (!HasJoined) return;
            appearance.SetGlasses(id);
        }

        public void SetShirt(string id)
        {
            if (!HasJoined) return;
            appearance.SetShirt(id);
        }

        public void SetPant(string id)
        {
            if (!HasJoined) return;
            appearance.SetPant(id);
        }

        public void SetShoe(string id)
        {
            if (!HasJoined) return;
            appearance.SetShoe(id);
        }

        public void SetEyes(string id)
        {
            if (!HasJoined) return;
            appearance.SetEyes(id);
        }

        public void SetSkin(string id)
        {
            if (!HasJoined) return;
            appearance.SetSkin(id);
        }

        public void SetHairColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetHairHSV(h, s, v);
        }

        public void SetPantColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetPantHSV(h, s, v);
        }

        public void SetEyeColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetEyeHSV(h, s, v);
        }

        public void SetSkinColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetSkinHSV(h, s, v);
        }

        public void ApplyServerAppearance(
            Guid id, 
            PlayerAppearanceSnapshot snapshot)
        {
            if (!HasJoined || id != PlayerID) return;
            appearance.ApplySnapshot(snapshot);
        }

        public void NormalizeAppearance(
            PlayerAppearanceSnapshot snapshot,
            PlayerAppearanceSnapshot defaults)
        {
            if (!HasJoined) return;
            appearance.ApplyNormalizedSnapshot(snapshot, defaults);
        }
        #endregion

        #region Inventory
        public void SwapInventory(int from, int to)
        {
            if (!HasJoined) return;
            inventory.Swap(from, to);
        }

        public void SetInventoryItem(int index, InventoryItemSnapshot item)
        {
            if (!HasJoined) return;
            inventory.SetItem(index, item);
        }

        public bool PickUpItem(InventoryItemSnapshot item)
        {
            if (!HasJoined) return false;
            return inventory.PickUpItem(item.ItemId, item.Quantity);
        }

        public void SelectInventorySlot(int index)
        {
            if (!HasJoined) return;
            inventory.SelectInventorySlot(index);
        }

        public bool RemoveSelectedItem(int quantity = 1)
        {
            return inventory.RemoveSelectedItem(quantity);
        }
        #endregion

        #region REMOVE LATER!!!!
        public void OnWorldObjectPlace(Vector2 position, string itemId)
        {
            if (!HasJoined) return;
            OnWorldObjectPlaced?.Invoke(position, itemId);
        }
        #endregion
    }
}
