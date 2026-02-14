using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignInView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button signInButton;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField emailTextField;
    [SerializeField] private TMP_InputField passwordTextField;
    #endregion

    #region Properties
    public event Action OnSignInClicked;

    public event Action<string> OnEmailChanged;
    public event Action<string> OnPasswordChanged;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        signInButton.onClick.AddListener(() => OnSignInClicked?.Invoke());

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
    #endregion
}
