using Assets.Gameplay.Entity.GrayShroom;
using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class GrayShroomBinder : ComponentBinder
{
    #region Attributes
    [Header("Appearance Catalogs")]
    [SerializeField] private EntityPartCatalog appearance;

    [SerializeField]
    private GrayShroomEntityView grayShroomEntityView;
    private GrayShroomEntityPresenter grayShroomEntityPresenter;

    private EntityService entityService;
    private SettingService settingService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Spawning Gray Shroom"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
        yield return BindWhenReady<EntityService>(entity =>
        {
            entityService = entity;
        });

        yield return BindWhenReady<SettingService>(setting =>
        {
            settingService = setting;
        });

        // Resolve dependencies
        grayShroomEntityPresenter = new GrayShroomEntityPresenter(
            entityService,
            settingService,
            grayShroomEntityView,

            appearance);

        GameLogger.Info(
            Channel.System,
            "Gray Shroom components bound successfully");
    }

    private void OnDestroy()
    {
        grayShroomEntityPresenter?.Dispose();
    }
    #endregion
}
