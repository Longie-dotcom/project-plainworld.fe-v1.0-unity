using Assets.State;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button logoutButton;
    #endregion

    #region Properties
    public event Action OnLogoutClicked;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        logoutButton.onClick.AddListener(() => OnLogoutClicked?.Invoke());
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
