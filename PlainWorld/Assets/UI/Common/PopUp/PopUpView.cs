using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpView : MonoBehaviour
{
    #region Attributes
    [Header("Icons")]
    [SerializeField] private Sprite errorIcon;
    [SerializeField] private Sprite informationIcon;
    [SerializeField] private Sprite questionIcon;
    [SerializeField] private Image icon;

    [Header("Buttons")]
    [SerializeField] private Button okButton;
    [SerializeField] private Button cancelButton;

    [Header("TextField")]
    [SerializeField] private TMP_Text text;
    #endregion

    #region Properties
    public event Action OnOkClicked;
    public event Action OnCancelClicked;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        okButton.onClick.AddListener(() => OnOkClicked?.Invoke());
        cancelButton.onClick.AddListener(() => OnCancelClicked?.Invoke());
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowInformation()
    {
        gameObject.SetActive(true);
        icon.sprite = informationIcon;
        cancelButton.gameObject.SetActive(false);
    }

    public void ShowError()
    {
        gameObject.SetActive(true);
        icon.sprite = errorIcon;
        cancelButton.gameObject.SetActive(false);
    }

    public void ShowQuestion()
    {
        gameObject.SetActive(true);
        icon.sprite = questionIcon;
        cancelButton.gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetMessage(string message)
    {
        text.text = message;
    }
    #endregion
}
