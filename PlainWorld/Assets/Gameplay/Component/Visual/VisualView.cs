using Assets.Data.Enum;
using Assets.State.Interface.State;
using System.Collections.Generic;
using UnityEngine;

public class VisualPart
{
    public SpriteRenderer Renderer;
    public EntityPartFrame Frame;
    public bool IsItem;

    public bool IsValid
    {
        get { return Renderer != null; }
    }
}

public class VisualView : MonoBehaviour
{
    #region Attributes
    [SerializeField] private float attackAnimDuration = 1f;
    [SerializeField] private int itemUseFrame = 1;

    protected readonly List<VisualPart> bodyParts = new();
    protected readonly List<VisualPart> itemParts = new();

    protected EntityAction currentAction;
    protected EntityDirection currentDirection;
    protected float animationTimer;
    protected float animationSpeedMultiplier;
    protected float currentSpeed = 0f;

    private float actionLockTimer = 0f;
    private bool actionLocked = false;
    private bool itemUsed = false;
    private bool pendingItemHide = false;
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
        UpdateActionLock();
        ApplySprite();
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public virtual void SetDirection(Vector2 dir)
    {
        currentDirection = DirFromVector(dir);
    }

    public virtual void SetAction(EntityAction action)
    {
        if (actionLocked)
            return;

        currentAction = action;

        if (action == EntityAction.ATTACK)
        {
            actionLocked = true;
            actionLockTimer = attackAnimDuration;
            animationTimer = 0f;
            itemUsed = false;
            pendingItemHide = false;
        }
    }

    public void ApplySettings(IReadOnlySettingState settings)
    {
        animationSpeedMultiplier = settings.AnimationSpeedMultiplier;
    }

    protected void ApplySprite()
    {
        RenderParts(bodyParts, "BodyPart");
        RenderParts(itemParts, "ItemPart");
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

    protected void AddItemPart(SpriteRenderer renderer, EntityPartFrame frame)
    {
        itemParts.Add(new VisualPart
        {
            Renderer = renderer,
            Frame = frame,
            IsItem = true
        });
    }

    protected void AddBodyPart(SpriteRenderer renderer, EntityPartFrame frame)
    {
        bodyParts.Add(new VisualPart
        {
            Renderer = renderer,
            Frame = frame,
            IsItem = false
        });
    }

    protected virtual void OnItemAfterUsed()
    {
        // Base does nothing
    }

    private void RenderParts(List<VisualPart> parts, string name)
    {
        if (parts.Count == 0) return;

        float speed =
            currentAction == EntityAction.ATTACK
                ? 1f
                : currentSpeed * animationSpeedMultiplier;

        animationTimer += Time.deltaTime * speed;

        var reference = parts.Find(p => p.IsValid);
        if (reference == null) return;

        int frame;
        if (currentAction == EntityAction.ATTACK)
        {
            int totalFrames = reference.Frame.FramesPerAction;
            float progress = Mathf.Clamp01(1f - (actionLockTimer / attackAnimDuration));
            frame = Mathf.FloorToInt(progress * totalFrames);
            frame = Mathf.Clamp(frame, 0, totalFrames - 1);

            // mark item for hiding later
            if (!itemUsed && frame >= itemUseFrame)
            {
                itemUsed = true;
                pendingItemHide = true;
            }
        }
        else
        {
            frame = Mathf.FloorToInt(animationTimer) % reference.Frame.FramesPerAction;
        }

        foreach (var part in parts)
        {
            if (!part.IsValid) continue;

            Sprite sprite = part.Frame.GetSprite(currentAction, currentDirection, frame);

            if (part.IsItem)
            {
                if (currentAction == EntityAction.ATTACK && part.Renderer.enabled)
                    part.Renderer.sprite = sprite;
                else
                    part.Renderer.sprite = null;
            }
            else
            {
                part.Renderer.sprite = sprite;
            }
        }
    }

    private void UpdateActionLock()
    {
        if (!actionLocked) return;

        actionLockTimer -= Time.deltaTime;

        if (actionLockTimer <= 0f)
        {
            actionLocked = false;
            currentAction = EntityAction.IDLE;
            animationTimer = 0f;

            if (pendingItemHide)
            {
                pendingItemHide = false;
                OnItemAfterUsed();
            }

            itemParts.Clear();
        }
    }
    #endregion
}
