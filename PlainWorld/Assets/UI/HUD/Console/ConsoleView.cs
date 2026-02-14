using Assets.State;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleView : MonoBehaviour
{
    #region Attributes
    [Header("Panels")]
    [SerializeField] private GameObject consolePanel;
    [SerializeField] private GameObject emojiPanel;

    [Header("Buttons")]
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button emojiButton;
    [SerializeField] private Button sendButton;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField inputTextField;

    [Header("Containers")]
    [SerializeField] private ConsoleContainer consoleContainer;
    [SerializeField] private EmojiContainer emojiContainer;

    [Header("Emojis")]
    [SerializeField] private EmojiCatalog emojiCatalog;

    private bool isOpen;
    private bool isEmojiOpen;
    private bool isParsing;
    #endregion

    #region Properties
    public event Action OnSendClicked;

    public event Action<string> OnInputChanged;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        openButton.onClick.AddListener(() =>
        {
            SetOpen(true);
        });
        closeButton.onClick.AddListener(() =>
        {
            SetOpen(false);
        });
        emojiButton.onClick.AddListener(() =>
        {
            SetEmojiOpen(!isEmojiOpen);
        });
        sendButton.onClick.AddListener(() => 
            OnSendClicked?.Invoke()
        );

        // Inputs
        inputTextField.onValueChanged.AddListener(HandleInputChanged);
        inputTextField.onSubmit.AddListener(_ => HandleInputSubmitted());

        // Containers
        emojiContainer.OnEmojiSelected += InsertEmoji;
    }

    void Start()
    {
        SetOpen(false);
        SetEmojiOpen(false);
    }

    void Update()
    {

    }

    public void HandleUIState(UIState state)
    {
        gameObject.SetActive(state.ShowHUD);
    }

    public void AppendMessage(string message)
    {
        consoleContainer.AddMessage(message);
    }

    private void SetEmojiOpen(bool value)
    {
        isEmojiOpen = value;
        emojiPanel.SetActive(isEmojiOpen);
    }

    private void SetOpen(bool value)
    {
        isOpen = value;

        consolePanel.SetActive(isOpen);
        openButton.gameObject.SetActive(!isOpen);
        closeButton.gameObject.SetActive(isOpen);
    }

    private void InsertEmoji(string unicode)
    {
        int caret = inputTextField.caretPosition;
        string text = inputTextField.text;

        text = text.Insert(caret, unicode);

        inputTextField.text = text;
        inputTextField.caretPosition = caret + unicode.Length;

        inputTextField.ActivateInputField();
    }

    private void HandleInputChanged(string value)
    {
        if (isParsing) return;

        isParsing = true;

        string raw = emojiCatalog.ParseToUnicode(value);
        string parsed = emojiCatalog.ParseToSpriteTags(raw);

        int oldCaret = inputTextField.caretPosition;

        inputTextField.SetTextWithoutNotify(parsed);

        // Only restore caret if it's NOT at the end
        if (oldCaret < parsed.Length)
        {
            inputTextField.caretPosition =
                Mathf.Clamp(oldCaret, 0, parsed.Length);
        }

        OnInputChanged?.Invoke(raw);

        isParsing = false;
    }

    private void HandleInputSubmitted()
    {
        if (string.IsNullOrWhiteSpace(inputTextField.text))
            return;

        // Convert back to raw before sending
        string raw = emojiCatalog.ParseToUnicode(inputTextField.text);

        OnInputChanged?.Invoke(raw);
        OnSendClicked?.Invoke();

        isParsing = true;

        inputTextField.SetTextWithoutNotify(string.Empty);
        inputTextField.caretPosition = 0;

        isParsing = false;

        inputTextField.ActivateInputField();    
    }
    #endregion
}
