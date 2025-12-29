using Assets.Gameplay.Entity.Player;
using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class EntityBinder : ComponentBinder
{
    #region Attributes
    [Header("Player Entity Appearance Catalogs")]
    [SerializeField] private EntityPartCatalog hairCatalog;
    [SerializeField] private EntityPartCatalog glassesCatalog;
    [SerializeField] private EntityPartCatalog shirtCatalog;
    [SerializeField] private EntityPartCatalog pantCatalog;
    [SerializeField] private EntityPartCatalog shoeCatalog;
    [SerializeField] private EntityPartCatalog eyesCatalog;
    [SerializeField] private EntityPartCatalog skinCatalog;

    [SerializeField]
    private PlayerEntityView entityPlayerView;
    private PlayerEntityPresenter entityPlayerPresenter;

    private EntityService entityService;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<EntityService>(entity =>
        {
            entityService = entity;
        });

        // Resolve dependencies
        entityPlayerPresenter = new PlayerEntityPresenter(
            entityService,
            entityPlayerView,

            hairCatalog,
            glassesCatalog,
            shirtCatalog,
            pantCatalog,
            shoeCatalog,
            eyesCatalog,
            skinCatalog);

        GameLogger.Info(
            Channel.System,
            "Player Entity components bound successfully");
    }

    private void OnDestroy()
    {
        entityPlayerPresenter?.Dispose();
    }
    #endregion
}
