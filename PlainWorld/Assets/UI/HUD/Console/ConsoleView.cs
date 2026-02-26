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

    private string inputText;
    #endregion

    #region Properties
    public event Action<string> OnSendClicked;
    #endregion

    #region Methods
    private void Awake()
    {
        openButton.onClick.AddListener(() => { 
            SetOpen(true); 
        });
        closeButton.onClick.AddListener(() => { 
            SetOpen(false); 
        });
        emojiButton.onClick.AddListener(()=> { 
            SetEmojiOpen(!isEmojiOpen); 
        });
        sendButton.onClick.AddListener(() => { 
            OnSendClicked?.Invoke(inputText); 
        });

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
        //gameObject.SetActive(state.ShowHUD);
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

        inputText = inputText.Insert(caret, unicode);

        isParsing = true;

        string parsed = emojiCatalog.ParseToSpriteTags(inputText);
        inputTextField.SetTextWithoutNotify(parsed);

        inputTextField.caretPosition = caret + unicode.Length;

        isParsing = false;
        inputTextField.ActivateInputField();
    }

    private void HandleInputChanged(string value)
    {
        if (isParsing)
            return;

        inputText = emojiCatalog.ParseToUnicode(value);
    }

    private void HandleInputSubmitted()
    {
        if (string.IsNullOrWhiteSpace(inputText))
            return;

        OnSendClicked?.Invoke(inputText);

        inputText = string.Empty;

        isParsing = true;
        inputTextField.SetTextWithoutNotify(string.Empty);
        inputTextField.caretPosition = 0;
        isParsing = false;

        inputTextField.ActivateInputField();
    }
    #endregion
}
