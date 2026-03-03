using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    #region Attributes
    public string Id;
    public string DisplayName;
    public Sprite Icon;
    public bool IsStackable;
    #endregion

    #region Properties
    #endregion

    #region Methods
    #endregion
}

[System.Serializable]
public class InventoryItem
{
    #region Attributes
    #endregion

    #region Properties
    public ItemSO Item { get; private set; }
    public int Quantity { get; private set; }
    #endregion

    public InventoryItem(ItemSO item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}