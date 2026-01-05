using Assets.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeCharacterView : MonoBehaviour
{
    #region Attributes
    [Header("Buttons")]
    [SerializeField] private Button finishButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button skinToLeftButton;
    [SerializeField] private Button skinToRightButton;

    [Header("Scrolls")]
    [SerializeField] private ScrollCollector hairScroll;
    [SerializeField] private ScrollCollector glassesScroll;
    [SerializeField] private ScrollCollector shirtScroll;
    [SerializeField] private ScrollCollector pantScroll;
    [SerializeField] private ScrollCollector shoeScroll;
    [SerializeField] private ScrollCollector eyesScroll;

    [Header("HSVs")]
    [SerializeField] private HSVCollector hairColorCollector;
    [SerializeField] private HSVCollector pantColorCollector;
    [SerializeField] private HSVCollector eyeColorCollector;
    [SerializeField] private HSVCollector skinColorCollector;

    [Header("Previews")]
    [SerializeField] private Image hair;
    [SerializeField] private Image glasses;
    [SerializeField] private Image shirt;
    [SerializeField] private Image pant;
    [SerializeField] private Image shoe;
    [SerializeField] private Image eyes;
    [SerializeField] private Image skin;
    #endregion

    #region Properties
    public event Action OnFinishClicked;
    public event Action OnBackClicked;
    public event Action OnSkinToLeftClicked;
    public event Action OnSkinToRightClicked;

    public event Action<string> OnHairChanged;
    public event Action<string> OnGlassesChanged;
    public event Action<string> OnShirtChanged;
    public event Action<string> OnPantChanged;
    public event Action<string> OnShoeChanged;
    public event Action<string> OnEyesChanged;

    public event Action<Color> OnHairColorChanged;
    public event Action<Color> OnPantColorChanged;
    public event Action<Color> OnEyeColorChanged;
    public event Action<Color> OnSkinColorChanged;
    #endregion

    #region Methods
    void Awake()
    {
        // Button
        finishButton.onClick.AddListener(() => OnFinishClicked?.Invoke());
        backButton.onClick.AddListener(() => OnBackClicked?.Invoke());
        skinToLeftButton.onClick.AddListener(() => OnSkinToLeftClicked?.Invoke());
        skinToRightButton.onClick.AddListener(() => OnSkinToRightClicked?.Invoke());

        // Scrolls
        hairScroll.OnValueChanged += v => OnHairChanged?.Invoke(v);
        glassesScroll.OnValueChanged += v => OnGlassesChanged?.Invoke(v);
        shirtScroll.OnValueChanged += v => OnShirtChanged?.Invoke(v);
        pantScroll.OnValueChanged += v => OnPantChanged?.Invoke(v);
        shoeScroll.OnValueChanged += v => OnShoeChanged?.Invoke(v);
        eyesScroll.OnValueChanged += v => OnEyesChanged?.Invoke(v);

        // Colors
        hairColorCollector.OnColorChanged += c => OnHairColorChanged?.Invoke(c);
        pantColorCollector.OnColorChanged += c => OnPantColorChanged?.Invoke(c);
        eyeColorCollector.OnColorChanged += c => OnEyeColorChanged?.Invoke(c);
        skinColorCollector.OnColorChanged += c => OnSkinColorChanged?.Invoke(c);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void HandleUIState(UIState state)
    {
        gameObject.SetActive(state.ShowCustomizeCharacter);
    }

    public void ApplyCurrentSelection(
        string hairId,
        string glassesId,
        string shirtId,
        string pantId,
        string shoeId,
        string eyesId,

        Color hairColor,
        Color pantColor,
        Color eyeColor,
        Color skinColor)
    {
        hairScroll.SetCurrentByID(hairId);
        glassesScroll.SetCurrentByID(glassesId);
        shirtScroll.SetCurrentByID(shirtId);
        pantScroll.SetCurrentByID(pantId);
        shoeScroll.SetCurrentByID(shoeId);
        eyesScroll.SetCurrentByID(eyesId);

        hairColorCollector.SetCurrentByColor(hairColor);
        pantColorCollector.SetCurrentByColor(pantColor);
        eyeColorCollector.SetCurrentByColor(eyeColor);
        skinColorCollector.SetCurrentByColor(skinColor);
    }

    public void SetHairValues(List<ScrollValue> values, int startIndex = 0)
    {
        hairScroll.SetValues(values, startIndex);
    }

    public void SetGlassesValues(List<ScrollValue> values, int startIndex = 0)
    {
        glassesScroll.SetValues(values, startIndex);
    }

    public void SetShirtValues(List<ScrollValue> values, int startIndex = 0)
    {
        shirtScroll.SetValues(values, startIndex);
    }

    public void SetPantValues(List<ScrollValue> values, int startIndex = 0)
    {
        pantScroll.SetValues(values, startIndex);
    }

    public void SetShoeValues(List<ScrollValue> values, int startIndex = 0)
    {
        shoeScroll.SetValues(values, startIndex);
    }

    public void SetEyesValues(List<ScrollValue> values, int startIndex = 0)
    {
        eyesScroll.SetValues(values, startIndex);
    }

    public void SetHairPreview(Sprite sprite, Color color)
    {
        hair.sprite = sprite;
        hair.color = color;
        hair.enabled = sprite != null;
    }

    public void SetGlassesPreview(Sprite sprite)
    {
        glasses.sprite = sprite;
        glasses.enabled = sprite != null;
    }

    public void SetShirtPreview(Sprite sprite)
    {
        shirt.sprite = sprite;
        shirt.enabled = sprite != null;
    }

    public void SetPantPreview(Sprite sprite, Color color)
    {
        pant.sprite = sprite;
        pant.color = color;
        pant.enabled = sprite != null;
    }

    public void SetShoePreview(Sprite sprite)
    {
        shoe.sprite = sprite;
        shoe.enabled = sprite != null;
    }

    public void SetEyesPreview(Sprite sprite, Color color)
    {
        eyes.sprite = sprite;
        eyes.color = color;
        eyes.enabled = sprite != null;
    }

    public void SetSkinPreview(Sprite sprite, Color color)
    {
        skin.sprite = sprite;
        skin.color = color;
        skin.enabled = sprite != null;
    }

    public void SetBackButtonVisible(bool visible)
    {
        if (backButton != null)
        {
            backButton.gameObject.SetActive(visible);
        }
    }

    public void SetFinishButtonEnabled(bool enabled)
    {
        if (finishButton == null)
            return;

        finishButton.interactable = enabled;
    }
    #endregion
}
