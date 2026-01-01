using UnityEngine;

public class PlayerVisualView : VisualView
{
    #region Attributes
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private SpriteRenderer glassesRenderer;
    [SerializeField] private SpriteRenderer shirtRenderer;
    [SerializeField] private SpriteRenderer pantRenderer;
    [SerializeField] private SpriteRenderer shoeRenderer;
    [SerializeField] private SpriteRenderer eyeRenderer;
    [SerializeField] private SpriteRenderer skinRenderer;
    #endregion

    #region Properties
    #endregion

    #region Methods
    public void ApplyAppearance(
        EntityPartFrame hair,
        EntityPartFrame glasses,
        EntityPartFrame shirt,
        EntityPartFrame pant,
        EntityPartFrame shoe,
        EntityPartFrame eyes,
        EntityPartFrame skin,

        Color hairColor,
        Color pantColor,
        Color eyeColor,
        Color skinColor)
    {
        parts.Clear();

        AddPart(hairRenderer, hair);
        AddPart(glassesRenderer, glasses);
        AddPart(shirtRenderer, shirt);
        AddPart(pantRenderer, pant);
        AddPart(shoeRenderer, shoe);
        AddPart(eyeRenderer, eyes);
        AddPart(skinRenderer, skin);

        hairRenderer.color = hairColor;
        pantRenderer.color = pantColor;
        eyeRenderer.color = eyeColor;
        skinRenderer.color = skinColor;

        animationTimer = 0f;
    }

    private void AddPart(SpriteRenderer renderer, EntityPartFrame frame)
    {
        if (renderer == null || frame == null) return;

        parts.Add(new VisualPart
        {
            Renderer = renderer,
            Frame = frame
        });
    }

    #endregion
}

