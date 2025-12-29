using Assets.Data.Enum;
using Assets.Service;
using Assets.State.Game;
using Assets.State.Player;
using Assets.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.UI.MainMenu.CustomizeCharacter
{
    public class CustomizeCharacterPresenter
    {
        #region Attributes
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
        private readonly EntityPartCatalog skinCatalog;

        private EntityDirection direction = EntityDirection.DOWN;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public CustomizeCharacterPresenter(
            PlayerService playerService,
            UIService uiService,
            GameService gameService,
            CustomizeCharacterView customizeCharacterView,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog,
            EntityPartCatalog skinCatalog)
        {
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
            this.skinCatalog = skinCatalog;

            Bind();

            customizeCharacterView.SetHairValues(BuildScrollValues(hairCatalog));
            customizeCharacterView.SetGlassesValues(BuildScrollValues(glassesCatalog));
            customizeCharacterView.SetShirtValues(BuildScrollValues(shirtCatalog));
            customizeCharacterView.SetPantValues(BuildScrollValues(pantCatalog));
            customizeCharacterView.SetShoeValues(BuildScrollValues(shoeCatalog));
            customizeCharacterView.SetEyesValues(BuildScrollValues(eyesCatalog));
        }

        #region Methods
        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            // Inbound
            customizeCharacterView.OnFinishClicked -= OnFinish;
            customizeCharacterView.OnBackClicked -= OnBack;

            customizeCharacterView.OnHairChanged -= OnHairChanged;
            customizeCharacterView.OnGlassesChanged -= OnGlassesChanged;
            customizeCharacterView.OnShirtChanged -= OnShirtChanged;
            customizeCharacterView.OnPantChanged -= OnPantChanged;
            customizeCharacterView.OnShoeChanged -= OnShoeChanged;
            customizeCharacterView.OnEyesChanged -= OnEyesChanged;

            customizeCharacterView.OnHairColorChanged -= OnHairColorChanged;
            customizeCharacterView.OnPantColorChanged -= OnPantColorChanged;
            customizeCharacterView.OnEyeColorChanged -= OnEyeColorChanged;
            customizeCharacterView.OnSkinColorChanged -= OnSkinColorChanged;

            // Outbound
            uiService.UIState.OnUIStateChanged -= customizeCharacterView.HandleUIState;
            playerService.PlayerState.OnAppearanceLoaded -= OnAppearanceLoaded;
            playerService.PlayerState.Appearance.OnChanged -= RefreshPreview;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(CustomizeCharacterPresenter));

            // Inbound
            customizeCharacterView.OnFinishClicked += OnFinish;
            customizeCharacterView.OnBackClicked += OnBack;

            customizeCharacterView.OnHairChanged += OnHairChanged;
            customizeCharacterView.OnGlassesChanged += OnGlassesChanged;
            customizeCharacterView.OnShirtChanged += OnShirtChanged;
            customizeCharacterView.OnPantChanged += OnPantChanged;
            customizeCharacterView.OnShoeChanged += OnShoeChanged;
            customizeCharacterView.OnEyesChanged += OnEyesChanged;

            customizeCharacterView.OnHairColorChanged += OnHairColorChanged;
            customizeCharacterView.OnPantColorChanged += OnPantColorChanged;
            customizeCharacterView.OnEyeColorChanged += OnEyeColorChanged;
            customizeCharacterView.OnSkinColorChanged += OnSkinColorChanged;

            // Outbound
            uiService.UIState.OnUIStateChanged += customizeCharacterView.HandleUIState;
            playerService.PlayerState.OnAppearanceLoaded += OnAppearanceLoaded;
            playerService.PlayerState.Appearance.OnChanged += RefreshPreview;
        }

        #region Buttons
        private void OnFinish()
        {
            AsyncHelper.Run(async () => 
            {
                await playerService.CreateAppearanceAsync();
            });
        }

        private void OnBack()
        {
            gameService.GameState.SetPhase(GamePhase.Login);
        }
        #endregion

        #region Scrolls
        private void OnHairChanged(string id)
        {
            playerService.PlayerState.SetHair(id);
        }

        private void OnGlassesChanged(string id)
        {
            playerService.PlayerState.SetGlasses(id);
        }

        private void OnShirtChanged(string id)
        {
            playerService.PlayerState.SetShirt(id);
        }

        private void OnPantChanged(string id)
        {
            playerService.PlayerState.SetPant(id);
        }

        private void OnShoeChanged(string id)
        {
            playerService.PlayerState.SetShoe(id);
        }

        private void OnEyesChanged(string id)
        {
            playerService.PlayerState.SetEyes(id);
        }
        #endregion

        #region HSV Collectors
        private void OnHairColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.PlayerState.SetHairColor(h, s, v);
        }

        private void OnPantColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.PlayerState.SetPantColor(h, s, v);
        }

        private void OnEyeColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.PlayerState.SetEyeColor(h, s, v);
        }

        private void OnSkinColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.PlayerState.SetSkinColor(h, s, v);
        }
        #endregion

        #region Outbound
        private void OnAppearanceLoaded(PlayerAppearance appearance)
        {
            // Ensure appearance has values
            appearance.EnsureDefaults(
                hairCatalog.GetDescriptors()[0].ID,
                glassesCatalog.GetDescriptors()[0].ID,
                shirtCatalog.GetDescriptors()[0].ID,
                pantCatalog.GetDescriptors()[0].ID,
                shoeCatalog.GetDescriptors()[0].ID,
                eyesCatalog.GetDescriptors()[0].ID,
                skinCatalog.GetDescriptors()[0].ID,

                Color.white,
                Color.white,
                Color.white,
                Color.white
            );

            // Apply on the UI components
            customizeCharacterView.ApplyCurrentSelection(
                appearance.HairID,
                appearance.GlassesID,
                appearance.ShirtID,
                appearance.PantID,
                appearance.ShoeID,
                appearance.EyesID,

                appearance.HairColor,
                appearance.PantColor,
                appearance.EyeColor,
                appearance.SkinColor
            );

            // Apply on preview
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            var a = playerService.PlayerState.Appearance;

            customizeCharacterView.SetHairPreview(
                GetSprite(hairCatalog, a.HairID), a.HairColor);

            customizeCharacterView.SetGlassesPreview(
                GetSprite(glassesCatalog, a.GlassesID));

            customizeCharacterView.SetShirtPreview(
                GetSprite(shirtCatalog, a.ShirtID));

            customizeCharacterView.SetPantPreview(
                GetSprite(pantCatalog, a.PantID), a.PantColor);

            customizeCharacterView.SetShoePreview(
                GetSprite(shoeCatalog, a.ShoeID));

            customizeCharacterView.SetEyesPreview(
                GetSprite(eyesCatalog, a.EyesID), a.EyeColor);

            customizeCharacterView.SetSkinPreview(
                GetSprite(skinCatalog, a.SkinID), a.SkinColor);
        }
        #endregion
        #endregion

        #region Private Helpers
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

        private Sprite GetSprite(EntityPartCatalog catalog, string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var descriptor = catalog.GetDescriptor(id);
                var sprite = descriptor != null ? descriptor
                    .Sprites[descriptor.FramesPerAction * (int)direction] : null;

                return sprite;
            }
            else
            {
                var descriptor = skinCatalog.GetDefault();
                var sprite = descriptor != null ? descriptor
                    .Sprites[descriptor.FramesPerAction * (int)direction] : null;

                return sprite;
            }
        }
        #endregion
    }
}
