using Assets.UI.Enum;
using UnityEngine;

public class CursorView : MonoBehaviour
{
    #region Attributes
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D hoverCursor;
    [SerializeField] private Texture2D clickCursor;
    [SerializeField] private Texture2D disabledCursor;
    [SerializeField] private Texture2D dragCursor;

    [SerializeField] private Vector2 hotSpot = Vector2.zero;

    private static CursorView instance;
    #endregion

    #region Properties
    #endregion

    #region Methods
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Apply(CursorType type)
    {
        Texture2D texture = type switch
        {
            CursorType.Hover => hoverCursor,
            CursorType.Click => clickCursor,
            CursorType.Disabled => disabledCursor,
            CursorType.Drag => dragCursor,
            _ => defaultCursor
        };

        Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
    }
    #endregion
}
