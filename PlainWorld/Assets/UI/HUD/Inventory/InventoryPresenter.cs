using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private InventoryView view;
    [SerializeField] private ItemCatalogSO catalog;
    [SerializeField] private int inventorySize = 16;

    private InventoryItem[] items;

    private void Start()
    {
        items = new InventoryItem[inventorySize];

        view.Initialize();
        view.SubscribeToDrop(OnItemDropped);

        LoadDummyData();
        RefreshView();
    }

    private void LoadDummyData()
    {
        if (catalog == null || catalog.Items.Length == 0)
            return;

        items[0] = new InventoryItem
        (
            catalog.Items[0], 1
        );

        if (catalog.Items.Length > 1)
        {
            items[1] = new InventoryItem
            (
                catalog.Items[1], 5
            );
        }
    }

    private void OnItemDropped(int fromIndex, int toIndex)
    {
        if (fromIndex == toIndex)
            return;

        if (!IsValid(fromIndex) || !IsValid(toIndex))
            return;

        var temp = items[fromIndex];
        items[fromIndex] = items[toIndex];
        items[toIndex] = temp;

        RefreshView();
    }

    private void RefreshView()
    {
        view.Bind(items);
    }

    private bool IsValid(int index)
    {
        return index >= 0 && index < items.Length;
    }
}