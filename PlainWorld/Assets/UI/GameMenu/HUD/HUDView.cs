using Assets.State;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button customizeCharacterButton;
    [SerializeField] private Button settingButton;
    #endregion

    #region Properties
    public event Action OnLogoutClicked;
    public event Action OnCustomizeCharacterClicked;
    public event Action OnSettingClicked;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        logoutButton.onClick.AddListener(() => OnLogoutClicked?.Invoke());
        customizeCharacterButton.onClick.AddListener(() => OnCustomizeCharacterClicked?.Invoke());
        settingButton.onClick.AddListener(() => OnSettingClicked?.Invoke());
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void HandleUIState(UIState state)
    {
        gameObject.SetActive(state.ShowHUD);
    }
    #endregion
}
