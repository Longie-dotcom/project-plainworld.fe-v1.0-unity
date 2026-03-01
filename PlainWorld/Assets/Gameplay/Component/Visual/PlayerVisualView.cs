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
    [SerializeField] private SpriteRenderer itemRenderer;
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
        bodyParts.Clear();

        AddBodyPart(hairRenderer, hair);
        AddBodyPart(glassesRenderer, glasses);
        AddBodyPart(shirtRenderer, shirt);
        AddBodyPart(pantRenderer, pant);
        AddBodyPart(shoeRenderer, shoe);
        AddBodyPart(eyeRenderer, eyes);
        AddBodyPart(skinRenderer, skin);

        hairRenderer.color = hairColor;
        pantRenderer.color = pantColor;
        eyeRenderer.color = eyeColor;
        skinRenderer.color = skinColor;

        animationTimer = 0f;
    }

    public void HoldItem(EntityPartFrame item)
    {
        itemParts.Clear();
        AddItemPart(itemRenderer, item);
        itemRenderer.enabled = true;
    }

    protected override void OnItemAfterUsed()
    {
        if (itemRenderer != null)
            itemRenderer.enabled = false;
    }
    #endregion
}

