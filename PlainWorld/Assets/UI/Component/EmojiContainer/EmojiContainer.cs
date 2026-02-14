using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmojiContainer : MonoBehaviour
{
    #region Attributes
    [Header("Scroll")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;

    [Header("Data")]
    [SerializeField] private EmojiCatalog database;
    [SerializeField] private EmojiItem itemPrefab;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField emojiSearchTextField;
    #endregion

    #region Properties
    public event System.Action<string> OnEmojiSelected;
    #endregion

    #region Methods
    void Awake()
    {
        emojiSearchTextField.onValueChanged.AddListener(HandleSearchChanged);
    }

    void Start()
    {
        Build();
    }

    void Update()
    {

    }

    private void Build(string filter = "")
    {
        // Clear old children
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        string lowerFilter = filter.ToLower();

        foreach (var emoji in database.emojis)
        {
            // If filter exists, skip non-matching emojis
            if (!string.IsNullOrEmpty(lowerFilter))
            {
                bool match =
                    emoji.id.ToLower().Contains(lowerFilter) ||
                    emoji.unicode.ToLower().Contains(lowerFilter);

                if (!match)
                    continue;
            }

            var item = Instantiate(itemPrefab, content);
            item.Initialize(emoji.icon, emoji.unicode);
            item.OnClicked += HandleEmojiClicked;
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void HandleEmojiClicked(string unicode)
    {
        OnEmojiSelected?.Invoke(unicode);
    }

    private void HandleSearchChanged(string value)
    {
        Build(value);
    }
    #endregion
}
