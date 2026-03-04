using Assets.Data.Enum;
using Assets.State.Interface.State;
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

    public void HoldItem(EntityPartFrame item)
    {
        visualView.HoldItem(item);
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
        Color skinColor,
        
        string name)
    {
        visualView.ApplyAppearance(
            hair, 
            glasses, 
            shirt, 
            pant, 
            shoe, 
            eyes, 
            skin,

            hairColor, 
            pantColor, 
            eyeColor, 
            skinColor,
            
            name);
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

    public void SetSpeed(float speed)
    {
        visualView.SetSpeed(speed);
    }

    public void ApplySettings(IReadOnlySettingState readOnlySettingState)
    {
        visualView.ApplySettings(readOnlySettingState);
    }
    #endregion
}

