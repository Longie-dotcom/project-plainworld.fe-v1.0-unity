using Assets.State;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HUDUtilsView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button customizeCharacterButton;
    [SerializeField] private Button settingButton;

    [Header("Panels")]
    [SerializeField] private CustomizeCharacterView customizeCharacterView;
    [SerializeField] private SettingView settingView;
    #endregion

    #region Properties
    public event Action OnLogoutClicked;
    #endregion

    #region Methods
    void Awake()
    {
        logoutButton.onClick.AddListener(() => OnLogoutClicked?.Invoke());
        customizeCharacterButton.onClick.AddListener(() => Open(customizeCharacterView, customizeCharacterButton));
        settingButton.onClick.AddListener(() => Open(settingView, settingButton));
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !IsTyping())
        {
            ToggleHUD();
        }
    }

    public void HandleUIState(UIState state)
    {
        gameObject.SetActive(state.ShowHUD);
    }

    private void Open(MonoBehaviour view, Button activeButton)
    {
        CloseAll();
        view.gameObject.SetActive(true);
        UpdateButtons(activeButton);
    }

    private void CloseAll()
    {
        customizeCharacterView.gameObject.SetActive(false);
        settingView.gameObject.SetActive(false);

        customizeCharacterButton.interactable = true;
        settingButton.interactable = true;
    }

    private void UpdateButtons(Button activeButton)
    {
        // Interactable
        customizeCharacterButton.interactable =
            customizeCharacterButton != activeButton;

        settingButton.interactable =
            settingButton != activeButton;
    }

    private void ToggleHUD()
    {
        bool isVisible = logoutButton.gameObject.activeSelf;

        if (isVisible)
        {
            CloseAll();
            SetButtonsVisible(false);
        }
        else
        {
            Open(customizeCharacterView, customizeCharacterButton);
            SetButtonsVisible(true);
        }
    }

    private void SetButtonsVisible(bool visible)
    {
        logoutButton.gameObject.SetActive(visible);
        customizeCharacterButton.gameObject.SetActive(visible);
        settingButton.gameObject.SetActive(visible);
    }

    private bool IsTyping()
    {
        if (EventSystem.current == null)
            return false;

        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null)
            return false;

        return selected.GetComponent<TMP_InputField>() != null;
    }
    #endregion
}
