using Assets.Data.Enum;
using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    #region Attributes
    [Header("Sub Views")]
    [SerializeField] private PlayerMoveView moveView;
    [SerializeField] private PlayerVisualView visualView;
    #endregion

    #region Properties
    public event Action<Vector2> OnUpdateVisualMove;
    public event Action OnSendMoveToServer;
    #endregion

    #region Methods
    private void Awake()
    {
        moveView.OnUpdateVisualMove += dir => OnUpdateVisualMove?.Invoke(dir);
        moveView.OnSendMoveToServer += () => OnSendMoveToServer?.Invoke();
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
            shirt,
            pant,
            shoe,
            eyes,
            skin,

            hairColor,
            pantColor,
            eyeColor,
            skinColor);
    }

    public void ApplyPosition(Vector2 pos)
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

    public void SetAnimationSpeed(float moveSpeed)
    {
        visualView.SetAnimationSpeed(moveSpeed);
    }
    #endregion
}

