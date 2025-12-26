using System;
using UnityEngine;

public class HSVCollector : MonoBehaviour
{
    #region Attributes
    [SerializeField] private BarDrag H;
    [SerializeField] private BarDrag S;
    [SerializeField] private BarDrag V;
    #endregion

    #region Properties
    public event Action<Color> OnColorChanged;
    #endregion

    #region Methods
    void Awake()
    {
        H.OnValueChanged += UpdateAll;
        S.OnValueChanged += UpdateAll;
        V.OnValueChanged += UpdateAll;

        UpdateAll();
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void UpdateAll()
    {
        Color color = Color.HSVToRGB(H.Value, S.Value, V.Value);

        H.UpdateGradient(H.Value, S.Value, V.Value);
        S.UpdateGradient(H.Value, S.Value, V.Value);
        V.UpdateGradient(H.Value, S.Value, V.Value);

        OnColorChanged?.Invoke(color);
    }
    #endregion
}

