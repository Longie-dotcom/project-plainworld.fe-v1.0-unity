using Assets.Data.Enum;
using UnityEngine;

[CreateAssetMenu(
    fileName = "EntityPartFrame", 
    menuName = "Animation/EntityPartFrame"
)]
public class EntityPartFrame : ScriptableObject
{
    #region Attributes
    #endregion

    #region Properties
    public string EntityPartName;
    public EntityPartType EntityPartType;

    public int FramesPerAction;
    public int DirectionCount = 4;

    // Flattened: [row * FramesPerAction + frame]
    public Sprite[] Sprites;
    public EntityAction[] SupportedActions;
    #endregion

    #region Methods
    public Sprite GetSprite(EntityAction action, EntityDirection direction, int frame)
    {
        if (SupportedActions == null || SupportedActions.Length == 0 || !HasAction(action))
            action = EntityAction.IDLE;

        int row = (int)action * DirectionCount + (int)direction;
        int index = row * FramesPerAction + frame;

        if (index < 0 || index >= Sprites.Length)
            return null;

        return Sprites[index];
    }

    public bool HasAction(EntityAction action)
    {
        for (int i = 0; i < SupportedActions.Length; i++)
        {
            if (SupportedActions[i] == action)
                return true;
        }
        return false;
    }
    #endregion
}
