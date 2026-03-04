using System;
using Assets.Service;
using Assets.State.Component.Player;

namespace Assets.UI.HUD.Inventory
{
    public class InventoryItemViewModel
    {
        public ItemSO Item { get; }
        public int Quantity { get; }

        public InventoryItemViewModel(ItemSO item, int quantity)
        {
            Item = item;
            Quantity = quantity;
        }
    }

    public class InventoryPresenter : IDisposable
    {
        #region Attributes
        private readonly PlayerService playerService;
        private readonly InventoryView inventoryView;

        private readonly ItemCatalogSO itemCatalog;

        private bool disposed;
        #endregion

        public InventoryPresenter(
            PlayerService playerService,
            InventoryView inventoryView,
            ItemCatalogSO itemCatalog)
        {
            this.playerService = playerService;
            this.inventoryView = inventoryView;
            this.itemCatalog = itemCatalog;

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            inventoryView.UnsubscribeToDrop(OnItemDropped);
            inventoryView.OnSelectedItemChanged -= OnSelectedItemChanged;

            // Outbound
            playerService.PlayerState.Inventory.OnInventoryChanged -= RefreshView;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(InventoryPresenter));

            // Inbound
            inventoryView.SubscribeToDrop(OnItemDropped);
            inventoryView.OnSelectedItemChanged += OnSelectedItemChanged;

            // Outbound
            playerService.PlayerState.Inventory.OnInventoryChanged += RefreshView;
        }

        private void OnItemDropped(int fromIndex, int toIndex)
        {
            // Invalid drop (item dropped outside valid slots)
            if (fromIndex == toIndex)
            {
                RefreshView();
                return;
            }

            // Valid drop → swap in state
            playerService.SwapInventory(fromIndex, toIndex);
        }

        private void OnSelectedItemChanged(int index)
        {
            playerService.SelectInventorySlot(index);
        }

        private void RefreshView()
        {
            var items = playerService.PlayerState.Inventory.Items;

            var displayItems = new InventoryItemViewModel[items.Length];

            for (int i = 0; i < items.Length; i++)
            {
                var id = items[i]?.ItemId;

                if (string.IsNullOrEmpty(id)) continue;

                var quantity = items[i]?.Quantity ?? 0;

                // Lookup the actual SO
                var itemSO = id != null ? itemCatalog.GetById(id) : null;

                displayItems[i] = new InventoryItemViewModel(itemSO, quantity);
            }

            inventoryView.Bind(displayItems);
        }
        #endregion
    }
}