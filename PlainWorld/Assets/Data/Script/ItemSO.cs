using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    public string Id;
    public string DisplayName;
    public Sprite Icon;
    public bool IsStackable;
}

[System.Serializable]
public class InventoryItem
{
    public ItemSO Item;
    public int Quantity;

    public InventoryItem(ItemSO item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }
}