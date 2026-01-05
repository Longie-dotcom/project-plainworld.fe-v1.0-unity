using Assets.Service;
using Assets.UI.MainMenu.CustomizeCharacter;
using Assets.Utility;
using System.Collections;
using UnityEngine;

public class CustomizeCharacterBinder : ComponentBinder
{
    #region Attributes
    [Header("Catalogs")]
    [SerializeField] private EntityPartCatalog hairCatalog;
    [SerializeField] private EntityPartCatalog glassesCatalog;
    [SerializeField] private EntityPartCatalog shirtCatalog;
    [SerializeField] private EntityPartCatalog pantCatalog;
    [SerializeField] private EntityPartCatalog shoeCatalog;
    [SerializeField] private EntityPartCatalog eyesCatalog;
    [SerializeField] private EntityPartCatalog skinCatalog;

    [SerializeField]
    private CustomizeCharacterView customizeCharacterView;
    private CustomizeCharacterPresenter customizeCharacterPresenter;

    private PlayerService playerService;
    private UIService uiService;
    private GameService gameService;
    #endregion

    #region Properties
    public override string StepName
    {
        get { return "Customize Character UI"; }
    }
    #endregion

    #region Methods
    public override IEnumerator BindAllServices()
    {
        yield return BindWhenReady<UIService>(ui =>
        {
            uiService = ui;
        });

        yield return BindWhenReady<GameService>(game =>
        {
            gameService = game;
        });

        yield return BindWhenReady<PlayerService>(player =>
        {
            playerService = player;
        });

        // Resolve dependencies
        customizeCharacterPresenter = new CustomizeCharacterPresenter(
            uiService,
            gameService,
            playerService,
            customizeCharacterView,

            hairCatalog,
            glassesCatalog,
            shirtCatalog,
            pantCatalog,
            shoeCatalog,
            eyesCatalog,
            skinCatalog
        );

        GameLogger.Info(
            Channel.System,
            "Customize character UI components binded successfully");
    }

    private void OnDestroy()
    {
        customizeCharacterPresenter?.Dispose();
    }
    #endregion
}

