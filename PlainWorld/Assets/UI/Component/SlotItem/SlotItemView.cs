using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SlotItemView : MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private Transform originalParent;
    private bool wasDropped;

    public int SlotIndex { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Bind(InventoryItem item, int slotIndex)
    {
        SlotIndex = slotIndex;
        icon.sprite = item.Item.Icon;
        quantityText.text = item.Quantity > 1 ? item.Quantity.ToString() : "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        wasDropped = false;

        transform.SetParent(transform.root, true);

        SetPivotWithoutMoving(new Vector2(0.5f, 0.5f));

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (!wasDropped)
        {
            var slot = originalParent.GetComponent<SlotView>();
            slot.SetItemView(this);
        }

        SetPivotWithoutMoving(new Vector2(0f, 0f));
    }

    // Called by SlotView via eventData.pointerDrag
    public void MarkAsDropped()
    {
        wasDropped = true;
    }

    private void SetPivotWithoutMoving(Vector2 newPivot)
    {
        Vector3 worldPos = rectTransform.position;
        rectTransform.pivot = newPivot;
        rectTransform.position = worldPos;
    }
}