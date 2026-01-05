using Assets.Data.Enum;
using Assets.State.Interface.IReadOnlyState;
using System.Collections.Generic;
using UnityEngine;

public class VisualPart
{
    public SpriteRenderer Renderer;
    public EntityPartFrame Frame;

    public bool IsValid
    {
        get { return Renderer != null; }
    }
}

public class VisualView : MonoBehaviour
{
    #region Attributes
    protected readonly List<VisualPart> parts = new();

    protected EntityAction currentAction;
    protected EntityDirection currentDirection;
    protected float animationTimer;
    protected float animationSpeedMultiplier;
    protected float currentPlayerSpeed = 0f;
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
        ApplySprite();
    }

    public void SetPlayerSpeed(float speed)
    {
        currentPlayerSpeed = speed;
    }

    public virtual void SetDirection(Vector2 dir)
    {
        currentDirection = DirFromVector(dir);
    }

    public virtual void SetAction(EntityAction action)
    {
        currentAction = action;
    }

    protected void ApplySprite()
    {
        if (parts.Count == 0) return;

        float animationSpeed = currentPlayerSpeed * animationSpeedMultiplier;

        animationTimer += Time.deltaTime * animationSpeed;
        // Assume at least one valid frame defines timing
        var reference = parts.Find(p => p.IsValid);
        if (reference == null) return;

        int frame =
            Mathf.FloorToInt(animationTimer) %
            reference.Frame.FramesPerAction;

        foreach (var part in parts)
        {
            if (!part.IsValid) continue;

            part.Renderer.sprite =
                part.Frame.GetSprite(currentAction, currentDirection, frame);
        }
    }

    protected EntityDirection DirFromVector(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return currentDirection; // keep last facing

        if (Mathf.Abs(dir.y) >= Mathf.Abs(dir.x))
            return dir.y > 0 ? EntityDirection.UP : EntityDirection.DOWN;
        else
            return dir.x > 0 ? EntityDirection.RIGHT : EntityDirection.LEFT;
    }

    public void ApplySettings(IReadOnlySettingState settings)
    {
        animationSpeedMultiplier = settings.AnimationSpeedMultiplier;
    }
    #endregion
}
