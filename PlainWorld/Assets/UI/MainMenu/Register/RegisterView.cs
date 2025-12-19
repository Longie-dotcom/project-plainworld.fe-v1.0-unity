using Assets.State;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegisterView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button registerButton;
    [SerializeField] private Button femaleButton;
    [SerializeField] private Button maleButton;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField emailTextField;
    [SerializeField] private TMP_InputField passwordTextField;
    [SerializeField] private TMP_InputField fullNameTextField;
    [SerializeField] private TMP_InputField dobTextField;
    #endregion

    #region Properties
    public event Action OnRegisterClicked;
    public event Action OnFemaleClicked;
    public event Action OnMaleClicked;

    public event Action<string> OnEmailChanged;
    public event Action<string> OnPasswordChanged;
    public event Action<string> OnFullNameChanged;
    public event Action<string> OnDobChanged;
    #endregion

    public RegisterView() { }

    #region Methods
    void Awake()
    {
        // Buttons
        registerButton.onClick.AddListener(() => OnRegisterClicked?.Invoke());
        femaleButton.onClick.AddListener(() => OnFemaleClicked?.Invoke());
        registerButton.onClick.AddListener(() => OnMaleClicked?.Invoke());

        // Input
        emailTextField.onValueChanged.AddListener(v => OnEmailChanged?.Invoke(v));
        passwordTextField.onValueChanged.AddListener(v => OnPasswordChanged?.Invoke(v));
        fullNameTextField.onValueChanged.AddListener(v => OnFullNameChanged?.Invoke(v));
        dobTextField.onValueChanged.AddListener(v => OnDobChanged?.Invoke(v));
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
    #endregion
}
