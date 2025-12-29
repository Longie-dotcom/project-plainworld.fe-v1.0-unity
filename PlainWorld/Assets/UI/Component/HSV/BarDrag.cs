using Assets.UI.Enum;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarDrag : 
    MonoBehaviour,
    IPointerDownHandler,
    IDragHandler
{
    #region Attributes
    [SerializeField] private HSVChannel channel;
    [SerializeField] private RectTransform bar;
    [SerializeField] private RawImage barImage;
    [SerializeField] private RectTransform target;
    #endregion

    #region Properties
    public float Value { get; private set; }

    public event Action OnValueChanged;
    #endregion

    #region Methods
    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateDrag(eventData);
    }

    private void UpdateDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bar,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        float width = bar.rect.width;
        float x = Mathf.Clamp(localPoint.x, -width / 2f, width / 2f);

        target.localPosition = new Vector3(x, target.localPosition.y, 0f);

        float newValue = (x + width / 2f) / width;

        if (!Mathf.Approximately(Value, newValue))
        {
            Value = newValue;
            OnValueChanged?.Invoke();
        }
    }

    public void UpdateGradient(float h, float s, float v)
    {
        barImage.texture = channel switch
        {
            HSVChannel.Hue => GenerateHue(),
            HSVChannel.Saturation => GenerateSaturation(h, v),
            HSVChannel.Value => GenerateValue(h, s),
            _ => barImage.texture
        };
    }

    public void SetValue(float value)
    {
        Value = Mathf.Clamp01(value);

        float width = bar.rect.width;
        float x = Mathf.Lerp(-width / 2f, width / 2f, Value);

        target.localPosition = new Vector3(x, target.localPosition.y, 0f);
    }
    #endregion

    #region Private Helpers
    private Texture2D GenerateHue(int width = 256)
    {
        var tex = new Texture2D(width, 1, TextureFormat.RGBA32, false);
        for (int x = 0; x < width; x++)
            tex.SetPixel(x, 0, Color.HSVToRGB(x / (width - 1f), 1, 1));
        tex.Apply();
        return tex;
    }

    private Texture2D GenerateSaturation(float h, float v, int width = 256)
    {
        var tex = new Texture2D(width, 1, TextureFormat.RGBA32, false);
        for (int x = 0; x < width; x++)
            tex.SetPixel(x, 0, Color.HSVToRGB(h, x / (width - 1f), v));
        tex.Apply();
        return tex;
    }

    private Texture2D GenerateValue(float h, float s, int width = 256)
    {
        var tex = new Texture2D(width, 1, TextureFormat.RGBA32, false);
        for (int x = 0; x < width; x++)
            tex.SetPixel(x, 0, Color.HSVToRGB(h, s, x / (width - 1f)));
        tex.Apply();
        return tex;
    }
    #endregion
}

