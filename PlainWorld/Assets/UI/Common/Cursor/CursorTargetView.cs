using Assets.UI.Enum;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorTarget : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IEndDragHandler
{
    #region Attributes
    [Header("Cursor Types")]
    [SerializeField] private CursorType defaultType = CursorType.Default;
    [SerializeField] private CursorType hoverType = CursorType.Hover;
    [SerializeField] private CursorType clickType = CursorType.Click;
    [SerializeField] private CursorType dragType = CursorType.Drag;

    private Action<CursorType> requestCursor;
    #endregion

    #region Properties
    public static event Action<CursorTarget> OnTargetEnabled;
    public static event Action<CursorTarget> OnTargetDisabled;
    #endregion

    #region Methods
    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnEnable()
    {
        OnTargetEnabled?.Invoke(this);
    }

    void OnDisable()
    {
        OnTargetDisabled?.Invoke(this);
    }

    public void Bind(Action<CursorType> request)
    {
        requestCursor = request;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        requestCursor?.Invoke(hoverType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        requestCursor?.Invoke(defaultType);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        requestCursor?.Invoke(clickType);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        requestCursor?.Invoke(hoverType);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        requestCursor?.Invoke(dragType);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        requestCursor?.Invoke(defaultType);
    }
    #endregion
}
