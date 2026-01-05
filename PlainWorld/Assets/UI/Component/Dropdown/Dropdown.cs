using UnityEngine;
using UnityEngine.UI;

public class Dropdown : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Button toggleButton;
    [SerializeField] private GameObject dropdownBar;

    private bool isOpen = false;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private void Awake()
    {
        if (dropdownBar != null)
            dropdownBar.SetActive(false);

        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleDropdown);
    }

    private void OnDestroy()
    {
        if (toggleButton != null)
            toggleButton.onClick.RemoveListener(ToggleDropdown);
    }

    private void ToggleDropdown()
    {
        isOpen = !isOpen;
        dropdownBar.SetActive(isOpen);
    }

    public void Close()
    {
        isOpen = false;
        dropdownBar.SetActive(false);
    }

    public void Open()
    {
        isOpen = true;
        dropdownBar.SetActive(true);
    }
    #endregion
}
