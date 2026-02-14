using TMPro;
using UnityEngine;

public class ConsoleMessageView : MonoBehaviour
{
    #region Attributes
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private EmojiCatalog emojiCatalog;
    #endregion

    #region Properties
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

    public void Set(string message)
    {
        messageText.text = emojiCatalog.ParseToSpriteTags(message);
    }
    #endregion
}
