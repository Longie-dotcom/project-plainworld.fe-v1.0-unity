using Assets.Gameplay.Entity.EntityPlayer;
using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class EntityBinder : ComponentBinder
{
    #region Attributes
    [SerializeField]
    private EntityPlayerView entityPlayerPrefab;
    private EntityPlayerPresenter entityPlayerPresenter;
    private EntityService entityService;
    #endregion

    #region Properties
    #endregion

    public EntityBinder() { }

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<EntityService>(entity =>
        {
            entityService = entity;
        });

        // Resolve dependencies
        entityPlayerPresenter = new EntityPlayerPresenter(
            entityService,
            entityPlayerPrefab);

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
