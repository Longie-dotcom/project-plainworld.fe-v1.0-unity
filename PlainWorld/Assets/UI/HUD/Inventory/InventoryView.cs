using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private SlotView[] slots;
    [SerializeField] private SlotItemView slotItemPrefab;

    public void Initialize()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init(i);
        }
    }

    public void Bind(InventoryItem[] items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Clear();

            if (items == null || i >= items.Length || items[i] == null)
                continue;

            var itemView = Instantiate(slotItemPrefab, slots[i].transform, false);
            itemView.Bind(items[i], i);
            slots[i].SetItemView(itemView);
        }
    }

    public void SubscribeToDrop(System.Action<int, int> callback)
    {
        foreach (var slot in slots)
            slot.OnItemDropped += callback;
    }
}