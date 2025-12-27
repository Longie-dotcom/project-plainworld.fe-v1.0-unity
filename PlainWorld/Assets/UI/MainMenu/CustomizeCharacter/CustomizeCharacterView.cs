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

    [Header("Scrolls")]
    [SerializeField] private ScrollCollector hairScroll;
    [SerializeField] private ScrollCollector glassesScroll;
    [SerializeField] private ScrollCollector shirtScroll;
    [SerializeField] private ScrollCollector pantScroll;
    [SerializeField] private ScrollCollector shoeScroll;
    [SerializeField] private ScrollCollector eyesScroll;

    [Header("HSVs")]
    [SerializeField] private HSVCollector skinColor;
    [SerializeField] private HSVCollector hairColor;
    [SerializeField] private HSVCollector pantColor;
    [SerializeField] private HSVCollector eyeColor;
    #endregion

    #region Properties
    public event Action OnFinishClicked;
    public event Action OnBackClicked;

    public event Action<string> OnHairChanged;
    public event Action<string> OnGlassesChanged;
    public event Action<string> OnShirtChanged;
    public event Action<string> OnPantChanged;
    public event Action<string> OnShoeChanged;
    public event Action<string> OnEyesChanged;

    public event Action<Color> OnSkinColorChanged;
    public event Action<Color> OnHairColorChanged;
    public event Action<Color> OnPantColorChanged;
    public event Action<Color> OnEyeColorChanged;
    #endregion

    #region Methods
    void Awake()
    {
        // Button
        finishButton.onClick.AddListener(() => OnFinishClicked?.Invoke());
        backButton.onClick.AddListener(() => OnBackClicked?.Invoke());

        // Scrolls
        hairScroll.OnValueChanged += v => OnHairChanged?.Invoke(v);
        glassesScroll.OnValueChanged += v => OnGlassesChanged?.Invoke(v);
        shirtScroll.OnValueChanged += v => OnShirtChanged?.Invoke(v);
        pantScroll.OnValueChanged += v => OnPantChanged?.Invoke(v);
        shoeScroll.OnValueChanged += v => OnShoeChanged?.Invoke(v);
        eyesScroll.OnValueChanged += v => OnEyesChanged?.Invoke(v);

        // Colors
        skinColor.OnColorChanged += c => OnSkinColorChanged?.Invoke(c);
        hairColor.OnColorChanged += c => OnHairColorChanged?.Invoke(c);
        pantColor.OnColorChanged += c => OnPantColorChanged?.Invoke(c);
        eyeColor.OnColorChanged += c => OnEyeColorChanged?.Invoke(c);
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
    #endregion
}
