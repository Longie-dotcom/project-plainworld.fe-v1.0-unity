using Assets.Data.Enum;
using UnityEngine;

public class PlayerVisualView : MonoBehaviour
{
    #region Attributes
    [SerializeField] private EntityPartFrame hairFrame;
    [SerializeField] private SpriteRenderer hairRenderer;
    [SerializeField] private EntityPartFrame glassesFrame;
    [SerializeField] private SpriteRenderer glassesRenderer;
    [SerializeField] private EntityPartFrame shirtFrame;
    [SerializeField] private SpriteRenderer shirtRenderer;
    [SerializeField] private EntityPartFrame pantFrame;
    [SerializeField] private SpriteRenderer pantRenderer;
    [SerializeField] private EntityPartFrame shoeFrame;
    [SerializeField] private SpriteRenderer shoeRenderer;
    [SerializeField] private EntityPartFrame eyeFrame;
    [SerializeField] private SpriteRenderer eyeRenderer;
    [SerializeField] private EntityPartFrame skinFrame;
    [SerializeField] private SpriteRenderer skinRenderer;

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

    private void ApplySprite()
    {
        animationTimer += Time.deltaTime * animationSpeed;
        int frame = Mathf.FloorToInt(animationTimer) % skinFrame.FramesPerAction;

        Sprite hair = hairFrame.GetSprite(currentAction, currentDirection, frame);
        hairRenderer.sprite = hair;
        Sprite glasses = glassesFrame.GetSprite(currentAction, currentDirection, frame);
        glassesRenderer.sprite = glasses;
        Sprite shirt = shirtFrame.GetSprite(currentAction, currentDirection, frame);
        shirtRenderer.sprite = shirt;
        Sprite pant = pantFrame.GetSprite(currentAction, currentDirection, frame);
        pantRenderer.sprite = pant;
        Sprite shoe = shoeFrame.GetSprite(currentAction, currentDirection, frame);
        shoeRenderer.sprite = shoe;
        Sprite eye = eyeFrame.GetSprite(currentAction, currentDirection, frame);
        eyeRenderer.sprite = eye;
        Sprite skin = skinFrame.GetSprite(currentAction, currentDirection, frame);
        skinRenderer.sprite = skin;
    }

    public void SetAction(EntityAction action)
    {
        currentAction = action;
    }

    public void SetDirection(Vector2 dir)
    {
        currentDirection = DirFromVector(dir);
    }

    public void SetAnimationSpeed(float moveSpeed)
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
    #endregion
}

