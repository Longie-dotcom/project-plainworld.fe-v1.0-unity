using Assets.Gameplay.Player;
using Assets.Network;
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

    private NetworkService networkService;
    private PlayerService playerService;
    private GameService gameService;
    private UIService uiService;
    #endregion

    #region Properties
    #endregion

    #region Methods
    private IEnumerator Start()
    {
        yield return BindWhenReady<NetworkService>(network =>
        {
            networkService = network;
        });

        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        yield return BindWhenReady<GameService>(game =>
        {
            gameService = game;
        });

        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        // Resolve dependencies
        playerPresenter = new PlayerPresenter(
            networkService,
            playerService,
            gameService,
            uiService,
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
