using UnityEngine;

public class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private InventoryView view;
    [SerializeField] private ItemCatalogSO catalog;

    private InventoryItem[] items;
    private const int SIZE = 16;

    private void Start()
    {
        items = new InventoryItem[SIZE];

        LoadDummyData();

        view.Build(SIZE);
        view.SubscribeToSlotClicks(OnSlotClicked);

        Refresh();
    }

    private void LoadDummyData()
    {
        items[0] = new InventoryItem(catalog.Items[0], 1);
        items[1] = new InventoryItem(catalog.Items[1], 5);
    }

    private void Refresh()
    {
        for (int i = 0; i < SIZE; i++)
            view.BindSlot(i, items[i]);
    }

    private void OnSlotClicked(int index)
    {
        Debug.Log($"Clicked slot {index}");

        // Later:
        // service.UseItem(index);
        // service.MoveItem(...)
    }
}