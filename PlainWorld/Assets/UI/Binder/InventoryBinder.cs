using System.Collections;
using Assets.Service;
using Assets.UI.HUD.Inventory;
using Assets.Utility;
using UnityEngine;

public class InventoryBinder : ComponentBinder
{
    #region Attributes
    [Header("Catalogs")]
    [SerializeField] private ItemCatalogSO itemCatalogSO;

    [SerializeField]
    private InventoryView inventoryView;
    private InventoryPresenter inventoryPresenter;

    private PlayerService playerService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "HUD: Inventory UI"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        // Resolve dependencies
        inventoryPresenter = new InventoryPresenter(
            playerService,
            inventoryView,
            itemCatalogSO);

        inventoryPresenter.LoadDummyData();

        GameLogger.Info(
            Channel.System,
            "HUD: Inventory UI components binded successfully");
    }

    private void OnDestroy()
    {
        inventoryPresenter?.Dispose();
    }
    #endregion
}
