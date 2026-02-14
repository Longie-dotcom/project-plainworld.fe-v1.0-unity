using Assets.State;
using UnityEngine;
using UnityEngine.UI;

public class MenuUtilsView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signUpButton;
    [SerializeField] private Button settingButton;

    [Header("Panels")]
    [SerializeField] private SignInView signInView;
    [SerializeField] private SignUpView signUpView;
    [SerializeField] private SettingView settingView;
    #endregion

    #region Properties
    #endregion

    #region Methods
    void Awake()
    {
        signInButton.onClick.AddListener(() => Open(signInView, signInButton));
        signUpButton.onClick.AddListener(() => Open(signUpView, signUpButton));
        settingButton.onClick.AddListener(() => Open(settingView, settingButton));
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void HandleUIState(UIState state)
    {
        gameObject.SetActive(state.ShowMenu);
    }

    private void Open(MonoBehaviour view, Button activeButton)
    {
        CloseAll();
        view.gameObject.SetActive(true);
        UpdateButtons(activeButton);
    }

    public void CloseAll()
    {
        signInView.gameObject.SetActive(false);
        signUpView.gameObject.SetActive(false);
        settingView.gameObject.SetActive(false);

        signInButton.interactable = true;
        signUpButton.interactable = true;
        settingButton.interactable = true;
    }

    private void UpdateButtons(Button activeButton)
    {
        // Interactable
        signInButton.interactable = signInButton != activeButton;
        signUpButton.interactable = signUpButton != activeButton;
        settingButton.interactable = settingButton != activeButton;
    }
    #endregion
}
