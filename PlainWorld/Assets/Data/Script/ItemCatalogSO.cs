using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Catalog")]
public class ItemCatalogSO : ScriptableObject
{
    public ItemSO[] Items;

    public ItemSO GetById(string id)
    {
        foreach (var item in Items)
        {
            if (item.Id == id)
                return item;
        }
        return null;
    }
}