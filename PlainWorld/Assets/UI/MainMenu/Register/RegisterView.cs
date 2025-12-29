using Assets.State.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterView : MonoBehaviour
{
    #region Attributes
    [Header("Icons")]
    [SerializeField] private Sprite tickIcon;
    [SerializeField] private Sprite crossIcon;

    [SerializeField] private Image emailWarnIcon;
    [SerializeField] private Image passwordWarnIcon;
    [SerializeField] private Image genderWarnIcon;
    [SerializeField] private Image nameWarnIcon;
    [SerializeField] private Image dobWarnIcon;

    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button femaleButton;
    [SerializeField] private Button maleButton;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField emailTextField;
    [SerializeField] private TMP_InputField passwordTextField;
    [SerializeField] private TMP_InputField fullNameTextField;
    [SerializeField] private TMP_InputField dayDobTextField;
    [SerializeField] private TMP_InputField monthDobTextField;
    [SerializeField] private TMP_InputField yearDobTextField;
    #endregion

    #region Properties
    public event Action OnBackClicked;
    public event Action OnRegisterClicked;
    public event Action OnFemaleClicked;
    public event Action OnMaleClicked;

    public event Action<string> OnEmailChanged;
    public event Action<string> OnPasswordChanged;
    public event Action<string> OnFullNameChanged;
    public event Action<string> OnDayDobChanged;
    public event Action<string> OnMonthDobChanged;
    public event Action<string> OnYearDobChanged;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        registerButton.onClick.AddListener(() => OnRegisterClicked?.Invoke());
        femaleButton.onClick.AddListener(() => OnFemaleClicked?.Invoke());
        maleButton.onClick.AddListener(() => OnMaleClicked?.Invoke());

        // Inputs
        emailTextField.onValueChanged.AddListener(v => OnEmailChanged?.Invoke(v));
        passwordTextField.onValueChanged.AddListener(v => OnPasswordChanged?.Invoke(v));
        fullNameTextField.onValueChanged.AddListener(v => OnFullNameChanged?.Invoke(v));
        dayDobTextField.onValueChanged.AddListener(v => OnDayDobChanged?.Invoke(v));
        monthDobTextField.onValueChanged.AddListener(v => OnMonthDobChanged?.Invoke(v));
        yearDobTextField.onValueChanged.AddListener(v => OnYearDobChanged?.Invoke(v));
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void HandleUIState(UIState state)
    {
        gameObject.SetActive(state.ShowRegister);
    }

    public void SetRegisterInteractable(bool value)
    {
        registerButton.interactable = value;
    }

    public void SetEmailValid(bool isValid)
    {
        emailWarnIcon.enabled = true;
        emailWarnIcon.sprite = isValid ? tickIcon : crossIcon;
    }

    public void SetPasswordValid(bool isValid)
    {
        passwordWarnIcon.enabled = true;
        passwordWarnIcon.sprite = isValid ? tickIcon : crossIcon;
    }

    public void SetNameValid(bool isValid)
    {
        nameWarnIcon.enabled = true;
        nameWarnIcon.sprite = isValid ? tickIcon : crossIcon;
    }

    public void SetGenderValid(bool isValid)
    {
        genderWarnIcon.enabled = true;
        genderWarnIcon.sprite = isValid ? tickIcon : crossIcon;
    }

    public void SetDobValid(bool isValid)
    {
        dobWarnIcon.enabled = true;
        dobWarnIcon.sprite = isValid ? tickIcon : crossIcon;
    }
    #endregion
}
