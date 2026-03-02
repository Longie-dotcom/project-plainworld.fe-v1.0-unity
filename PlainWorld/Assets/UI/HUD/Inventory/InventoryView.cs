using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private SlotView slotPrefab;
    [SerializeField] private Transform slotRoot;

    private SlotView[] slots;

    public void Build(int size)
    {
        slots = new SlotView[size];

        for (int i = 0; i < size; i++)
        {
            var slot = Instantiate(slotPrefab, slotRoot);
            slot.Init(i);
            slots[i] = slot;
        }
    }

    public void BindSlot(int index, InventoryItem item)
    {
        slots[index].SetItem(item);
    }

    public void SubscribeToSlotClicks(System.Action<int> callback)
    {
        foreach (var slot in slots)
            slot.OnSlotClicked += callback;
    }
}