using UnityEngine;
using UnityEngine.EventSystems;

public class SlotView : MonoBehaviour, IDropHandler
{
    public int Index { get; private set; }

    private SlotItemView currentItem;

    public System.Action<int, int> OnItemDropped;

    public void Init(int index)
    {
        Index = index;
    }

    public void SetItemView(SlotItemView itemView)
    {
        currentItem = itemView;

        if (itemView != null)
        {
            itemView.transform.SetParent(transform);
            itemView.transform.localPosition = Vector3.zero;
        }
    }

    public void Clear()
    {
        if (currentItem != null)
            Destroy(currentItem.gameObject);

        currentItem = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag?.GetComponent<SlotItemView>();
        if (item == null) return;

        // ALWAYS reparent to this slot
        item.transform.SetParent(transform, false);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        item.MarkAsDropped();
    }
}