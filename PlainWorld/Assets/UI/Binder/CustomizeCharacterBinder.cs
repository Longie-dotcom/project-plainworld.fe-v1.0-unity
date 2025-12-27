using Assets.Service;
using Assets.UI.MainMenu.CustomizeCharacter;
using Assets.Utility;
using System.Collections;
using UnityEngine;

namespace Assets.UI.Binder
{
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

        [SerializeField]
        private CustomizeCharacterView customizeCharacterView;
        private CustomizeCharacterPresenter customizeCharacterPresenter;

        private AuthService authService;
        private PlayerService playerService;
        private UIService uiService;
        private GameService gameService;
        #endregion

        #region Properties
        #endregion

        #region Methods
        private IEnumerator Start()
        {
            yield return BindWhenReady<AuthService>(auth =>
            {
                authService = auth;
            });

            yield return BindWhenReady<PlayerService>(player =>
            {
                playerService = player;
            });

            yield return BindWhenReady<UIService>(ui =>
            {
                uiService = ui;
            });

            yield return BindWhenReady<GameService>(game =>
            {
                gameService = game;
            });

            // Resolve dependencies
            customizeCharacterPresenter = new CustomizeCharacterPresenter(
                authService,
                playerService,
                uiService,
                gameService,
                customizeCharacterView,

                hairCatalog,
                glassesCatalog,
                shirtCatalog,
                pantCatalog,
                shoeCatalog,
                eyesCatalog
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
}
