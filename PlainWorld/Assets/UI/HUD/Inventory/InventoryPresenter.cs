using System;
using Assets.Service;

namespace Assets.UI.HUD.Inventory
{
    public class InventoryPresenter : IDisposable
    {
        #region Attributes
        private readonly PlayerService playerService;
        private readonly InventoryView inventoryView;

        private readonly ItemCatalogSO itemCatalog;

        private readonly int inventorySize = 35;
        private InventoryItem[] items;

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

            items = new InventoryItem[inventorySize];

            Bind();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound

            // Outbound
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(InventoryPresenter));

            // Inbound

            // Outbound
            inventoryView.SubscribeToDrop(OnItemDropped);
        }

        public void LoadDummyData()
        {
            if (itemCatalog == null || itemCatalog.Items.Length == 0)
                return;

            items[0] = new InventoryItem(itemCatalog.Items[0], 1);
            items[1] = new InventoryItem(itemCatalog.Items[1], 1);
            items[2] = new InventoryItem(itemCatalog.Items[2], 5);

            RefreshView();
        }

        private void OnItemDropped(int fromIndex, int toIndex)
        {
            if (fromIndex == toIndex)
            {
                inventoryView.Bind(items);
                return;
            }

            if (!IsValid(fromIndex) || !IsValid(toIndex))
                return;

            var temp = items[fromIndex];
            items[fromIndex] = items[toIndex];
            items[toIndex] = temp;

            RefreshView();
        }

        private void RefreshView()
        {
            inventoryView.Bind(items);
        }

        private bool IsValid(int index)
        {
            return index >= 0 && index < items.Length;
        }
        #endregion
    }
}