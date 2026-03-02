using UnityEngine;
using UnityEngine.EventSystems;

public class SlotView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SlotItemView itemPrefab;

    private SlotItemView currentItem;
    private int index;

    public System.Action<int> OnSlotClicked;

    public void Init(int slotIndex)
    {
        index = slotIndex;
        Clear();
    }

    public void SetItem(InventoryItem item)
    {
        Clear();

        if (item == null)
            return;

        currentItem = Instantiate(itemPrefab, transform);
        currentItem.Bind(item.Item, item.Quantity);
    }

    public void Clear()
    {
        if (currentItem != null)
            Destroy(currentItem.gameObject);

        currentItem = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSlotClicked?.Invoke(index);
    }
}