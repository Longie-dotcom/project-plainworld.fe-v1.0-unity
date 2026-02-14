using UnityEngine;
using UnityEngine.UI;

public class ConsoleContainer : MonoBehaviour
{
    #region Attributes
    [Header("References")]
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform content;
    [SerializeField] private ConsoleMessageView messagePrefab;

    [Header("Settings")]
    [SerializeField] private int maxMessages = 100;
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

    public void AddMessage(string message)
    {
        // Create message object
        var item = Instantiate(messagePrefab, content);
        item.Set(message);

        // Trim messages 
        while (content.childCount > maxMessages)
        {
            Destroy(content.GetChild(0).gameObject);
        }

        // Check if message has reached the bottom
        if (scrollRect.verticalNormalizedPosition <= 0.001f)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
    #endregion
}
