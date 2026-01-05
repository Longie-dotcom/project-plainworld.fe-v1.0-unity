using Assets.Data.Enum;
using Assets.State.Interface.IReadOnlyState;
using UnityEngine;

public class PlayerEntityView : EntityView
{
    #region Attributes
    [SerializeField] private PlayerVisualView visualView;
    #endregion

    #region Properties
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
        visualView.ApplyAppearance(
            hair, 
            glasses, 
            shirt, pant, 
            shoe, 
            eyes, 
            skin,

            hairColor, 
            pantColor, 
            eyeColor, 
            skinColor);
    }

    public override void ApplyPosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, 0);
    }

    public void SetAction(EntityAction action)
    {
        visualView.SetAction(action);
    }

    public void SetDirection(Vector2 dir)
    {
        visualView.SetDirection(dir);
    }

    public void SetPlayerSpeed(float speed)
    {
        visualView.SetPlayerSpeed(speed);
    }

    public void ApplySettings(IReadOnlySettingState readOnlySettingState)
    {
        visualView.ApplySettings(readOnlySettingState);
    }
    #endregion
}

