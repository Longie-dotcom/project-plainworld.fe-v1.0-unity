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
    public event Action<Vector2> OnMove;
    public event Action OnStop;
    #endregion

    #region Methods
    private void Awake()
    {
        moveView.OnMove += dir => OnMove?.Invoke(dir);
        moveView.OnStop += () => OnStop?.Invoke();
    }

    void Start()
    {

    }

    void Update()
    {

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

