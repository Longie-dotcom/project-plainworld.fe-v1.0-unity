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
            var rect = itemView.GetComponent<RectTransform>();

            rect.SetParent(transform, false);

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            rect.pivot = new Vector2(0f, 0f);
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
        var dragged = eventData.pointerDrag?
            .GetComponent<SlotItemView>();

        if (dragged == null)
            return;

        // VISUAL MOVE IMMEDIATELY
        dragged.MarkAsDropped();
        OnItemDropped?.Invoke(dragged.SlotIndex, Index);
    }
}