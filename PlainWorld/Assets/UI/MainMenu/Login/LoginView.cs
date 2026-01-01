using Assets.State;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button joinButton;
    [SerializeField] private Button registerButton;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField emailTextField;
    [SerializeField] private TMP_InputField passwordTextField;
    #endregion

    #region Properties
    public event Action OnJoinClicked;
    public event Action OnRegisterClicked;

    public event Action<string> OnEmailChanged;
    public event Action<string> OnPasswordChanged;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        joinButton.onClick.AddListener(() => OnJoinClicked?.Invoke());
        registerButton.onClick.AddListener(() => OnRegisterClicked?.Invoke());

        // Inputs
        emailTextField.onValueChanged.AddListener(v => OnEmailChanged?.Invoke(v));
        passwordTextField.onValueChanged.AddListener(v => OnPasswordChanged?.Invoke(v));
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void HandleUIState(UIState state)
    {
        gameObject.SetActive(state.ShowLogin);
    }
    #endregion
}
