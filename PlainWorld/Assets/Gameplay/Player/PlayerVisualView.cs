using Assets.Data.Enum;
using UnityEngine;

public class PlayerVisualView : MonoBehaviour
{
    #region Attributes
    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private SpriteRenderer glassesRenderer;
    [SerializeField] private SpriteRenderer shirtRenderer;
    [SerializeField] private SpriteRenderer pantRenderer;
    [SerializeField] private SpriteRenderer shoeRenderer;
    [SerializeField] private SpriteRenderer eyeRenderer;
    [SerializeField] private SpriteRenderer skinRenderer;

    private EntityPartFrame hairFrame;
    private EntityPartFrame glassesFrame;
    private EntityPartFrame shirtFrame;
    private EntityPartFrame pantFrame;
    private EntityPartFrame shoeFrame;
    private EntityPartFrame eyeFrame;
    private EntityPartFrame skinFrame;

    private EntityAction currentAction;
    private EntityDirection currentDirection;
    private float animationTimer;
    private float animationSpeed = 8 / 5f;
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

    internal void ApplyAppearance(
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
        hairFrame = hair;
        glassesFrame = glasses;
        shirtFrame = shirt;
        pantFrame = pant;
        shoeFrame = shoe;
        eyeFrame = eyes;
        skinFrame = skin;

        hairRenderer.color = hairColor;
        pantRenderer.color = pantColor;
        eyeRenderer.color = eyeColor;
        skinRenderer.color = skinColor;

        animationTimer = 0f;
    }

    internal void SetAction(EntityAction action)
    {
        currentAction = action;
    }

    internal void SetDirection(Vector2 dir)
    {
        currentDirection = DirFromVector(dir);
    }

    internal void SetAnimationSpeed(float moveSpeed)
    {
        float baseSpeed = 8 / 5f;
        animationSpeed = baseSpeed * moveSpeed;
    }

    private EntityDirection DirFromVector(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return currentDirection; // keep the latest direction

        if (Mathf.Abs(dir.y) >= Mathf.Abs(dir.x))
            return dir.y > 0 ? EntityDirection.UP : EntityDirection.DOWN;
        else
            return dir.x > 0 ? EntityDirection.RIGHT : EntityDirection.LEFT;
    }

    private void ApplySprite()
    {
        animationTimer += Time.deltaTime * animationSpeed;
        int frame = Mathf.FloorToInt(animationTimer) % skinFrame.FramesPerAction;

        hairRenderer.sprite = hairFrame.GetSprite(currentAction, currentDirection, frame);
        glassesRenderer.sprite = glassesFrame.GetSprite(currentAction, currentDirection, frame);
        shirtRenderer.sprite = shirtFrame.GetSprite(currentAction, currentDirection, frame);
        pantRenderer.sprite = pantFrame.GetSprite(currentAction, currentDirection, frame);
        shoeRenderer.sprite = shoeFrame.GetSprite(currentAction, currentDirection, frame);
        eyeRenderer.sprite = eyeFrame.GetSprite(currentAction, currentDirection, frame);
        skinRenderer.sprite = skinFrame.GetSprite(currentAction, currentDirection, frame);
    }
    #endregion
}

