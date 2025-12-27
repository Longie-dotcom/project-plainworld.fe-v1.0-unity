using Assets.Service;
using Assets.State;
using Assets.UI.MainMenu.Login;
using System;
using System.Collections.Generic;

namespace Assets.UI.MainMenu.CustomizeCharacter
{
    public class CustomizeCharacterPresenter
    {
        #region Attributes
        private readonly AuthService authService;
        private readonly PlayerService playerService;
        private readonly UIService uiService;
        private readonly GameService gameService;

        private readonly CustomizeCharacterView customizeCharacterView;

        private readonly EntityPartCatalog hairCatalog;
        private readonly EntityPartCatalog glassesCatalog;
        private readonly EntityPartCatalog shirtCatalog;
        private readonly EntityPartCatalog pantCatalog;
        private readonly EntityPartCatalog shoeCatalog;
        private readonly EntityPartCatalog eyesCatalog;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public CustomizeCharacterPresenter(
            AuthService authService,
            PlayerService playerService,
            UIService uiService,
            GameService gameService,
            CustomizeCharacterView customizeCharacterView,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog)
        {
            this.authService = authService;
            this.playerService = playerService;
            this.uiService = uiService;
            this.gameService = gameService;
            this.customizeCharacterView = customizeCharacterView;

            this.hairCatalog = hairCatalog;
            this.glassesCatalog = glassesCatalog;
            this.shirtCatalog = shirtCatalog;
            this.pantCatalog = pantCatalog;
            this.shoeCatalog = shoeCatalog;
            this.eyesCatalog = eyesCatalog;

            Bind();
            InitializeScrolls();
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            uiService.UIState.OnUIStateChanged -= customizeCharacterView.HandleUIState;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(LoginPresenter));

            uiService.UIState.OnUIStateChanged += customizeCharacterView.HandleUIState;
        }

        private void OnFinish()
        {

        }

        private void OnBack()
        {
            gameService.GameState.SetPhase(GamePhase.Login);
        }

        private void InitializeScrolls()
        {
            customizeCharacterView.SetHairValues(BuildScrollValues(hairCatalog));
            customizeCharacterView.SetGlassesValues(BuildScrollValues(glassesCatalog));
            customizeCharacterView.SetShirtValues(BuildScrollValues(shirtCatalog));
            customizeCharacterView.SetPantValues(BuildScrollValues(pantCatalog));
            customizeCharacterView.SetShoeValues(BuildScrollValues(shoeCatalog));
            customizeCharacterView.SetEyesValues(BuildScrollValues(eyesCatalog));
        }

        private List<ScrollValue> BuildScrollValues(EntityPartCatalog catalog)
        {
            var list = new List<ScrollValue>();

            foreach (var descriptor in catalog.GetDescriptors())
            {
                list.Add(new ScrollValue
                {
                    ID = descriptor.ID,
                    Name = descriptor.Name,
                });
            }

            return list;
        }
        #endregion
    }
}
