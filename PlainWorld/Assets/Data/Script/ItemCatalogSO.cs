using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Catalog")]
public class ItemCatalogSO : ScriptableObject
{
    #region Attributes
    public ItemSO[] Items;
    #endregion

    #region Properties
    #endregion

    #region Methods
    public ItemSO GetById(string id)
    {
        foreach (var item in Items)
        {
            if (item.Id == id)
                return item;
        }
        return null;
    }
    #endregion
}