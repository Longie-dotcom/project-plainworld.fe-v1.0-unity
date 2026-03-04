using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotView : MonoBehaviour, IDropHandler
{
    #region Attributes
    [SerializeField] private Button button;

    private SlotItemView currentItem;
    #endregion

    #region Properties
    public int Index { get; private set; }

    public System.Action<int, int> OnItemDropped;
    public System.Action<int> OnClicked;
    #endregion

    #region Methods
    public void Init(int index)
    {
        Index = index;

        if (button != null)
            button.onClick.AddListener(HandleClick);
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

        dragged.MarkAsDropped();
        OnItemDropped?.Invoke(dragged.SlotIndex, Index);
    }

    public void SetSelected(bool selected)
    {
        if (button == null)
            return;

        if (selected)
            button.Select();
    }

    public bool HasItem()
    {
        return currentItem != null;
    }

    private void HandleClick()
    {
        OnClicked?.Invoke(Index);
    }
    #endregion
}