using UnityEngine;

public class GrayShroomVisualView : VisualView
{
    #region Attributes
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer appearanceRenderer;

    #endregion

    #region Properties
    #endregion

    #region Methods
    public void ApplyAppearance(EntityPartFrame appearance)
    {
        bodyParts.Clear();

        AddBodyPart(appearanceRenderer, appearance);

        animationTimer = 0f;
    }
    #endregion
}

