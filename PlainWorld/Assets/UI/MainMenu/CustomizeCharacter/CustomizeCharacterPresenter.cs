using Assets.Data.Enum;
using Assets.Service;
using Assets.Service.Enum;
using Assets.State.Component.Player;
using Assets.UI.Enum;
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
            customizeCharacterView.OnFinishClicked -= OnFinishClicked;
            customizeCharacterView.OnBackClicked -= OnBackClicked;
            customizeCharacterView.OnSkinToLeftClicked -= OnSkinToLeftClicked;
            customizeCharacterView.OnSkinToRightClicked -= OnSkinToRightClicked;

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
            playerService.PlayerState.OnPlayerCustomization -= OnPlayerCustomization;
            playerService.PlayerState.Appearance.OnChanged -= RefreshPreview;
        }

        private void Bind()
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(CustomizeCharacterPresenter));

            // Inbound
            customizeCharacterView.OnFinishClicked += OnFinishClicked;
            customizeCharacterView.OnBackClicked += OnBackClicked;
            customizeCharacterView.OnSkinToLeftClicked += OnSkinToLeftClicked;
            customizeCharacterView.OnSkinToRightClicked += OnSkinToRightClicked;

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
            playerService.PlayerState.OnPlayerCustomization += OnPlayerCustomization;
            playerService.PlayerState.Appearance.OnChanged += RefreshPreview;
        }

        #region Buttons
        private void OnFinishClicked()
        {
            AsyncHelper.Run(async () => 
            {
                try
                {
                    // Request create appearance
                    await playerService.CreateAppearanceAsync();

                    // Show success and return to login view
                    uiService.ShowPopUp(
                        PopUpType.Information,
                        "Registration successful!"
                    );

                    // Show success and return to previous view
                    uiService.ShowPopUp(
                        PopUpType.Information,
                        "Change character successful!"
                    );
                    gameService.PopPhase();
                }
                catch (Exception)
                {
                    uiService.ShowPopUp(
                        PopUpType.Error,
                        "Unexpected error. Please try again."
                    );
                }
            });
        }

        private void OnBackClicked()
        {
            gameService.PopPhase();
        }

        private void OnSkinToLeftClicked()
        {
            RotateDirection(-1);
        }

        private void OnSkinToRightClicked()
        {
            RotateDirection(+1);
        }

        private void RotateDirection(int delta)
        {
            int count = 4;

            int value = ((int)direction + delta) % count;
            if (value < 0) value += count;

            direction = (EntityDirection)value;

            RefreshPreview();
        }
        #endregion

        #region Scrolls
        private void OnHairChanged(string id)
        {
            playerService.SetHair(id);
        }

        private void OnGlassesChanged(string id)
        {
            playerService.SetGlasses(id);
        }

        private void OnShirtChanged(string id)
        {
            playerService.SetShirt(id);
        }

        private void OnPantChanged(string id)
        {
            playerService.SetPant(id);
        }

        private void OnShoeChanged(string id)
        {
            playerService.SetShoe(id);
        }

        private void OnEyesChanged(string id)
        {
            playerService.SetEyes(id);
        }
        #endregion

        #region HSV Collectors
        private void OnHairColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.SetHairColor(h, s, v);
        }

        private void OnPantColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.SetPantColor(h, s, v);
        }

        private void OnEyeColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.SetEyeColor(h, s, v);
        }

        private void OnSkinColorChanged(Color color)
        {
            var (h, s, v) = ColorHelper.ColorToHSV(color);
            playerService.SetSkinColor(h, s, v);
        }
        #endregion

        #region Outbound
        private void OnPlayerCustomization(PlayerAppearance appearance)
        {
            // Change state to show the customization UI
            gameService.PushPhase(GamePhase.CustomizeCharacter);

            // Prevent user ignores customization when there has no customization before
            customizeCharacterView.SetBackButtonVisible(appearance.IsCreated);

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
                var descriptor = catalog.GetPartFrame(id);
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
