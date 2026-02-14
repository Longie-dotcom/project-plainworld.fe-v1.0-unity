using System;
using UnityEngine;
using UnityEngine.UI;

public class EmojiItem : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    private string unicode;
    #endregion

    #region Properties
    public event Action<string> OnClicked;
    #endregion

    #region Methods
    void Start()
    {

    }

    void Awake()
    {

    }

    void Update()
    {

    }

    public void Initialize(Sprite sprite, string unicode)
    {
        icon.sprite = sprite;
        this.unicode = unicode;

        button.onClick.AddListener(() =>
        {
            OnClicked?.Invoke(this.unicode);
        });
    }
    #endregion
}
