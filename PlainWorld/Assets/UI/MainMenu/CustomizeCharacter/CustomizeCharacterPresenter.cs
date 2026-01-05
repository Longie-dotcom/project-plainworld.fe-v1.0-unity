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
    public class CharacterAppearancePreview
    {
        public string HairID;
        public string GlassesID;
        public string ShirtID;
        public string PantID;
        public string ShoeID;
        public string EyesID;
        public string SkinID;

        public Color HairColor;
        public Color PantColor;
        public Color EyeColor;
        public Color SkinColor;

        public CharacterAppearancePreview Clone()
        {
            return (CharacterAppearancePreview)MemberwiseClone();
        }
    }

    public class CustomizeCharacterPresenter
    {
        #region Attributes
        private readonly UIService uiService;
        private readonly GameService gameService;
        private readonly PlayerService playerService;
        private readonly CustomizeCharacterView customizeCharacterView;

        private readonly EntityPartCatalog hairCatalog;
        private readonly EntityPartCatalog glassesCatalog;
        private readonly EntityPartCatalog shirtCatalog;
        private readonly EntityPartCatalog pantCatalog;
        private readonly EntityPartCatalog shoeCatalog;
        private readonly EntityPartCatalog eyesCatalog;
        private readonly EntityPartCatalog skinCatalog;

        private EntityDirection direction = EntityDirection.DOWN;
        private CharacterAppearancePreview preview;
        private CharacterAppearancePreview original;
        private bool isCreated;

        private bool disposed;
        #endregion

        #region Properties
        #endregion

        public CustomizeCharacterPresenter(
            UIService uiService,
            GameService gameService,
            PlayerService playerService,
            CustomizeCharacterView customizeCharacterView,

            EntityPartCatalog hairCatalog,
            EntityPartCatalog glassesCatalog,
            EntityPartCatalog shirtCatalog,
            EntityPartCatalog pantCatalog,
            EntityPartCatalog shoeCatalog,
            EntityPartCatalog eyesCatalog,
            EntityPartCatalog skinCatalog)
        {
            this.uiService = uiService;
            this.gameService = gameService;
            this.playerService = playerService;
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
        }

        #region Buttons
        private void OnFinishClicked()
        {
            AsyncHelper.Run(async () => 
            {
                try
                {
                    // Commit changes
                    CommitPreview();

                    // Request create appearance
                    await playerService.CreateAppearanceAsync();

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
            // Ignore all changes
            preview = null;
            original = null;

            // Back to previous phase
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
            if (preview == null) return;
            preview.HairID = id;
            RefreshPreview();
        }

        private void OnGlassesChanged(string id)
        {
            if (preview == null) return;
            preview.GlassesID = id;
            RefreshPreview();
        }

        private void OnShirtChanged(string id)
        {
            if (preview == null) return;
            preview.ShirtID = id;
            RefreshPreview();
        }

        private void OnPantChanged(string id)
        {
            if (preview == null) return;
            preview.PantID = id;
            RefreshPreview();
        }

        private void OnShoeChanged(string id)
        {
            if (preview == null) return;
            preview.ShoeID = id;
            RefreshPreview();
        }

        private void OnEyesChanged(string id)
        {
            if (preview == null) return;
            preview.EyesID = id;
            RefreshPreview();
        }
        #endregion

        #region HSV Collectors
        private void OnHairColorChanged(Color color)
        {
            if (preview == null) return;
            preview.HairColor = color;
            RefreshPreview();
        }

        private void OnPantColorChanged(Color color)
        {
            if (preview == null) return;
            preview.PantColor = color;
            RefreshPreview();
        }

        private void OnEyeColorChanged(Color color)
        {
            if (preview == null) return;
            preview.EyeColor = color;
            RefreshPreview();
        }

        private void OnSkinColorChanged(Color color)
        {
            if (preview == null) return;
            preview.SkinColor = color;
            RefreshPreview();
        }
        #endregion

        #region Outbound
        private void OnPlayerCustomization(PlayerAppearance appearance)
        {
            // Change state to show the customization UI
            gameService.PushPhase(GamePhase.CustomizeCharacter);

            // Clone preview
            original = FromAppearance(appearance);
            preview = original.Clone();

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

        private void CommitPreview()
        {
            playerService.SetHair(preview.HairID);
            playerService.SetGlasses(preview.GlassesID);
            playerService.SetShirt(preview.ShirtID);
            playerService.SetPant(preview.PantID);
            playerService.SetShoe(preview.ShoeID);
            playerService.SetEyes(preview.EyesID);
            playerService.SetSkin(preview.SkinID);

            var (h, s, v) = ColorHelper.ColorToHSV(preview.HairColor);
            playerService.SetHairColor(h, s, v);

            (h, s, v) = ColorHelper.ColorToHSV(preview.PantColor);
            playerService.SetPantColor(h, s, v);

            (h, s, v) = ColorHelper.ColorToHSV(preview.EyeColor);
            playerService.SetEyeColor(h, s, v);

            (h, s, v) = ColorHelper.ColorToHSV(preview.SkinColor);
            playerService.SetSkinColor(h, s, v);
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

        private CharacterAppearancePreview FromAppearance(PlayerAppearance a)
        {
            return new CharacterAppearancePreview
            {
                HairID = a.HairID,
                GlassesID = a.GlassesID,
                ShirtID = a.ShirtID,
                PantID = a.PantID,
                ShoeID = a.ShoeID,
                EyesID = a.EyesID,
                SkinID = a.SkinID,

                HairColor = a.HairColor,
                PantColor = a.PantColor,
                EyeColor = a.EyeColor,
                SkinColor = a.SkinColor
            };
        }

        private void RefreshPreview()
        {
            var a = preview;
            if (a == null) return;

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

            customizeCharacterView.SetFinishButtonEnabled(CanFinish());
        }

        private bool IsDirty()
        {
            if (preview == null || original == null)
                return false;

            return
                preview.HairID != original.HairID ||
                preview.GlassesID != original.GlassesID ||
                preview.ShirtID != original.ShirtID ||
                preview.PantID != original.PantID ||
                preview.ShoeID != original.ShoeID ||
                preview.EyesID != original.EyesID ||

                preview.HairColor != original.HairColor ||
                preview.PantColor != original.PantColor ||
                preview.EyeColor != original.EyeColor ||
                preview.SkinColor != original.SkinColor;
        }

        private bool CanFinish()
        {
            // First time: always allowed
            if (!isCreated)
                return true;

            // Editing existing character: must change something
            return IsDirty();
        }
        #endregion
    }
}
