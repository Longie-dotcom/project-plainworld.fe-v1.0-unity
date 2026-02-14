using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button smallScreenButton;
    [SerializeField] private Button mediumScreenButton;
    [SerializeField] private Button fullScreenButton;
    #endregion

    #region Properties
    public event Action OnSmallScreenClicked;
    public event Action OnMediumScreenClicked;
    public event Action OnFullScreenClicked;
    #endregion

    #region Methods
    void Awake()
    {
        // Buttons
        smallScreenButton.onClick.AddListener(() => OnSmallScreenClicked?.Invoke());
        mediumScreenButton.onClick.AddListener(() => OnMediumScreenClicked?.Invoke());
        fullScreenButton.onClick.AddListener(() => OnFullScreenClicked?.Invoke());
    }

    void Start()
    {

    }

    void Update()
    {

    }
    #endregion
}
