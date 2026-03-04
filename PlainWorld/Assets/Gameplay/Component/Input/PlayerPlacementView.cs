using UnityEngine;
using System;

public class PlayerPlacementView : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Camera mainCamera;
    private bool isActive = false;
    private PlaceableItemSO currentItem;

    private GameObject currentGhost;
    private SpriteRenderer ghostRenderer;

    private const float TILE_SIZE = 1f; // match StaticObjectVisualView
    #endregion

    #region Properties
    public event Action<Vector2, string> OnPlaceItem; // position, itemId
    #endregion

    #region Methods
    public void Activate(PlaceableItemSO item)
    {
        isActive = true;
        currentItem = item;

        if (currentGhost != null)
            currentGhost.SetActive(true);
    }

    public void Deactivate()
    {
        isActive = false;
        currentItem = null;

        if (currentGhost != null)
            currentGhost.SetActive(false);
    }

    void Update()
    {
        if (!isActive || currentItem == null) return;

        Vector2 mouseWorld = GetMouseWorldPosition();
        Vector2 snappedPosition = SnapToGrid(mouseWorld);

        // Draw ghost preview at snapped position
        DrawPreview(snappedPosition, currentItem);

        if (Input.GetMouseButtonDown(0))
        {
            // Send placement action
            OnPlaceItem?.Invoke(snappedPosition, currentItem.Id);
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Cancel placement
            Deactivate();
        }
    }

    private Vector2 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.forward, Vector3.zero); // Z=0 plane
        if (plane.Raycast(ray, out float enter))
        {
            Vector3 hit = ray.GetPoint(enter);
            return new Vector2(hit.x, hit.y);
        }
        return Vector2.zero;
    }

    private Vector2 SnapToGrid(Vector2 worldPos)
    {
        float x = Mathf.Floor(worldPos.x / TILE_SIZE) * TILE_SIZE + TILE_SIZE * 0.5f; // center X
        float y = Mathf.Floor(worldPos.y / TILE_SIZE) * TILE_SIZE + TILE_SIZE * 0.5f; // bottom Y
        return new Vector2(x, y);
    }

    private void DrawPreview(Vector2 position, PlaceableItemSO item)
    {
        if (item == null)
            return;

        // Spawn ghost if not exists
        if (currentGhost == null)
        {
            currentGhost = new GameObject("GhostItem");
            ghostRenderer = currentGhost.AddComponent<SpriteRenderer>();
            ghostRenderer.sortingOrder = 100; // always on top
        }

        // Update sprite if item changed
        if (ghostRenderer.sprite != item.Icon)
            ghostRenderer.sprite = item.Icon;

        // Move ghost to snapped position
        currentGhost.transform.position = new Vector3(position.x, position.y, 0f);

        // Optional: collision check
        Vector2 size = item.Icon.bounds.size;
        bool canPlace = !Physics2D.OverlapBox(
            currentGhost.transform.position,
            size,
            0f,
            LayerMask.GetMask("PlacedObjects", "Obstacles")
        );

        ghostRenderer.color = canPlace
            ? new Color(1f, 1f, 1f, 0.5f)
            : new Color(1f, 0f, 0f, 0.5f);
    }

    /// <summary>
    /// Instantiates a placed item in the world at the given snapped grid position.
    /// </summary>
    public GameObject InstantiatePlacedItem(Vector2 position, PlaceableItemSO item)
    {
        if (item == null || item.Prefab == null)
            return null;

        Vector2 snapped = SnapToGrid(position);

        GameObject instance = GameObject.Instantiate(item.Prefab);
        instance.name = $"Placed_{item.Id}";
        instance.transform.position = new Vector3(snapped.x, snapped.y, 0f);

        // Optional: add parent container
        // instance.transform.SetParent(placedItemsParent);

        return instance;
    }
    #endregion
}