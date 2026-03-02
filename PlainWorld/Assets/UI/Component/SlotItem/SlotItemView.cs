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

        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        rectTransform.localPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        if (!wasDropped)
        {
            // Return to original slot
            transform.SetParent(originalParent);
            rectTransform.localPosition = Vector3.zero;
        }
    }

    // Called by SlotView via eventData.pointerDrag
    public void MarkAsDropped()
    {
        wasDropped = true;
    }
}