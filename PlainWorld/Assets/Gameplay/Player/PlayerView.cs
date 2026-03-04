using Assets.Data.Enum;
using Assets.Gameplay.Component.Visual;
using Assets.State.Interface.State;
using System;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    #region Attributes
    [Header("Sub Views")]
    [SerializeField] private PlayerActView actView;
    [SerializeField] private PlayerVisualView visualView;
    [SerializeField] private PlayerPlacementView placementView;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CameraBounds cameraBounds;
    #endregion

    #region Properties
    public event Action<Vector2, EntityAction> OnUpdateVisualAction;
    public event Action OnSendActionToServer;
    public event Action<Vector2, string> OnPlaceItemAction;
    #endregion

    #region Methods
    private void Awake()
    {
        actView.OnUpdateVisualAction += (Vector2 dir, EntityAction action) => OnUpdateVisualAction?.Invoke(dir, action);
        actView.OnSendActionToServer += () => OnSendActionToServer?.Invoke();

        placementView.OnPlaceItem += (Vector2 pos, string itemId) =>
        {
            OnPlaceItemAction?.Invoke(pos, itemId);
        };
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

    public void ActivatePlacement(PlaceableItemSO item)
    {
        placementView.Activate(item);
    }

    public void DeactivatePlacement()
    {
        placementView.Deactivate();
    }

    public void SetSpeed(float moveSpeed)
    {
        visualView.SetSpeed(moveSpeed);
    }

    public void ApplyPosition(Vector2 pos)
    {
        Vector3 targetPos = new Vector3(pos.x, pos.y, 0);
        transform.position = targetPos;

        FollowCamera(targetPos);
    }

    public void SetDirection(Vector2 dir)
    {
        visualView.SetDirection(dir);
    }

    public void SetAction(EntityAction action)
    {
        visualView.SetAction(action);
    }

    public void ApplySettings(IReadOnlySettingState settings)
    {
        visualView.ApplySettings(settings);
        actView.ApplySettings(settings);
    }

    public void InstantiatePlacedItem(Vector2 position, PlaceableItemSO item)
    {
        placementView.InstantiatePlacedItem(position, item);
    }

    #region Private Helper
    private void FollowCamera(Vector3 playerPos)
    {
        if (mainCamera == null || cameraBounds == null) return;

        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float minX = cameraBounds.min.x + camWidth;
        float maxX = cameraBounds.max.x - camWidth;
        float minY = cameraBounds.min.y + camHeight;
        float maxY = cameraBounds.max.y - camHeight;

        Vector3 clamped = new Vector3(
            Mathf.Clamp(playerPos.x, minX, maxX),
            Mathf.Clamp(playerPos.y, minY, maxY),
            mainCamera.transform.position.z
        );

        // ❗ Immediate stop at boundary
        mainCamera.transform.position = clamped;
    }
    #endregion
    #endregion
}

