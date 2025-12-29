using Assets.Gameplay.Player;
using Assets.Service;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class PlayerBinder : ComponentBinder
{
    #region Attributes
    [Header("Appearance Catalogs")]
    [SerializeField] private EntityPartCatalog hairCatalog;
    [SerializeField] private EntityPartCatalog glassesCatalog;
    [SerializeField] private EntityPartCatalog shirtCatalog;
    [SerializeField] private EntityPartCatalog pantCatalog;
    [SerializeField] private EntityPartCatalog shoeCatalog;
    [SerializeField] private EntityPartCatalog eyesCatalog;
    [SerializeField] private EntityPartCatalog skinCatalog;

    [SerializeField]
    private PlayerView playerView;
    private PlayerPresenter playerPresenter;

    private PlayerService playerService;
    private GameService gameService;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        yield return BindWhenReady<GameService>(game =>
        {
            gameService = game;
        });

        // Resolve dependencies
        playerPresenter = new PlayerPresenter(
            playerService,
            gameService,
            playerView,

            hairCatalog,
            glassesCatalog,
            shirtCatalog,
            pantCatalog,
            shoeCatalog,
            eyesCatalog,
            skinCatalog);

        GameLogger.Info(
            Channel.System,
            "Player components bound successfully");
    }

    private void OnDestroy()
    {
        playerPresenter?.Dispose();
    }
    #endregion
}
