using Assets.Data.Enum;
using Assets.State.Interface.State;
using UnityEngine;

public class GrayShroomEntityView : EntityView
{
    #region Attributes
    [SerializeField] private GrayShroomVisualView visualView;
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

    public void ApplyAppearance(EntityPartFrame appearance)
    {
        visualView.ApplyAppearance(appearance);
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

