using Assets.State.Interface.Component.Player;
using Assets.Utility;
using System;

namespace Assets.State.Component.Player
{
    public class InventoryItemSnapshot
    {
        #region Properties
        public string ItemId { get; private set; }
        public int Quantity { get; private set; }
        #endregion

        public InventoryItemSnapshot(string itemId, int quantity)
        {
            ItemId = itemId;
            Quantity = quantity;
        }

        #region Methods
        public void AddQuantity(int amount)
        {
            Quantity += amount;
        }

        public void RemoveQuantity(int amount)
        {
            Quantity = Math.Max(0, Quantity - amount);
        }
        #endregion
    }

    public class Inventory : IReadOnlyInventory
    {
        #region Attributes
        private readonly InventoryItemSnapshot[] items;
        private int selectedSlot = 0;
        #endregion

        #region Properties
        public InventoryItemSnapshot[] Items
        {
            get { return items; } 
        }

        public InventoryItemSnapshot SelectedItem
        {
            get { return IsValid(selectedSlot) ? items[selectedSlot] : null; }
        }

        public event Action OnInventoryChanged;
        public event Action<int> OnSelectedInventorySlotChanged;
        #endregion

        public Inventory(int size)
        {
            items = new InventoryItemSnapshot[size];
        }

        #region Methods
        public void SetItem(int index, InventoryItemSnapshot item)
        {
            if (!IsValid(index)) return;

            items[index] = item;
            OnInventoryChanged?.Invoke();
        }

        public void Swap(int from, int to)
        {
            if (!IsValid(from) || !IsValid(to)) return;

            (items[from], items[to]) = (items[to], items[from]);
            OnInventoryChanged?.Invoke();
        }

        public bool PickUpItem(string itemId, int quantity = 1)
        {
            if (string.IsNullOrEmpty(itemId)) return false;

            // First: try to stack if the item exists and is stackable
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].ItemId == itemId)
                {
                    items[i].AddQuantity(quantity);
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            // Second: find first empty slot
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = new InventoryItemSnapshot(itemId, quantity);
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            // No slot available
            return false;
        }

        public void SelectInventorySlot(int index)
        {
            if (!IsValid(index)) return;

            selectedSlot = index;

            OnSelectedInventorySlotChanged?.Invoke(index);
        }

        public bool RemoveSelectedItem(int quantity = 1)
        {
            if (!IsValid(selectedSlot))
                return false;

            var item = items[selectedSlot];

            if (item == null)
                return false;

            item.RemoveQuantity(quantity);

            // If quantity becomes 0 → clear slot
            if (item.Quantity <= 0)
            {
                items[selectedSlot] = null;
            }

            OnInventoryChanged?.Invoke();
            return true;
        }

        private bool IsValid(int index)
            => index >= 0 && index < items.Length;
        #endregion
    }
}
