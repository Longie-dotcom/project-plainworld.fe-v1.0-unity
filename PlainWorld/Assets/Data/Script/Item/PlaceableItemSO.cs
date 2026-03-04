using UnityEngine;

[CreateAssetMenu(menuName = "Items/Placable Item")]
public class PlaceableItemSO : ItemSO
{
    [Header("Placement Data")]
    public GameObject Prefab;
    public Vector3 Offset;
    public bool SnapToGrid = true;

    [Header("Placement Restrictions")]
    public LayerMask PlacementLayer;  // Which layers it can be placed on
}