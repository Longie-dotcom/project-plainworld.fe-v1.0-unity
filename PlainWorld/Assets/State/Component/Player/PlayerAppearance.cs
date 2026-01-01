using Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent;
using Assets.Utility;
using System;
using UnityEngine;

namespace Assets.State.Component.Player
{
    public readonly struct PlayerAppearanceSnapshot
    {
        public readonly bool IsCreated;

        public readonly string HairID;
        public readonly string GlassesID;
        public readonly string ShirtID;
        public readonly string PantID;
        public readonly string ShoeID;
        public readonly string EyesID;
        public readonly string SkinID;

        public readonly Color HairColor;
        public readonly Color PantColor;
        public readonly Color EyeColor;
        public readonly Color SkinColor;

        public PlayerAppearanceSnapshot(
            bool isCreated,
            string hair,
            string glasses,
            string shirt,
            string pant,
            string shoe,
            string eyes,
            string skin,
            Color hairColor,
            Color pantColor,
            Color eyeColor,
            Color skinColor)
        {
            IsCreated = isCreated;

            HairID = hair;
            GlassesID = glasses;
            ShirtID = shirt;
            PantID = pant;
            ShoeID = shoe;
            EyesID = eyes;
            SkinID = skin;

            HairColor = hairColor;
            PantColor = pantColor;
            EyeColor = eyeColor;
            SkinColor = skinColor;
        }
    }

    public class PlayerAppearance : IReadOnlyPlayerAppearance
    {
        #region Attributes
        #endregion

        #region Properties
        public bool IsCreated { get; private set; } = false;

        public string HairID { get; private set; }
        public string GlassesID { get; private set; }
        public string ShirtID { get; private set; }
        public string PantID { get; private set; }
        public string ShoeID { get; private set; }
        public string EyesID { get; private set; }
        public string SkinID { get; private set; }

        public Color HairColor { get; private set; }
        public Color PantColor { get; private set; }
        public Color EyeColor { get; private set; }
        public Color SkinColor { get; private set; }

        public event Action OnChanged;
        #endregion

        public PlayerAppearance() { }

        #region Methods
        public void ApplySnapshot(PlayerAppearanceSnapshot s)
        {
            IsCreated = s.IsCreated;

            HairID = s.HairID;
            GlassesID = s.GlassesID;
            ShirtID = s.ShirtID;
            PantID = s.PantID;
            ShoeID = s.ShoeID;
            EyesID = s.EyesID;
            SkinID = s.SkinID;

            HairColor = s.HairColor;
            PantColor = s.PantColor;
            EyeColor = s.EyeColor;
            SkinColor = s.SkinColor;

            OnChanged?.Invoke();
        }

        public PlayerAppearanceSnapshot PrepareForCreation()
        {
            if (!IsCreated) MarkCreated();

            return new PlayerAppearanceSnapshot(
                IsCreated,

                HairID,
                GlassesID,
                ShirtID,
                PantID,
                ShoeID,
                EyesID,
                SkinID,

                HairColor,
                PantColor,
                EyeColor,
                SkinColor
            );
        }

        public void SetHair(string id)
        {
            HairID = id;
            OnChanged?.Invoke();
        }

        public void SetGlasses(string id)
        {
            GlassesID = id;
            OnChanged?.Invoke();
        }

        public void SetShirt(string id)
        {
            ShirtID = id;
            OnChanged?.Invoke();
        }

        public void SetPant(string id)
        {
            PantID = id;
            OnChanged?.Invoke();
        }

        public void SetShoe(string id)
        {
            ShoeID = id;
            OnChanged?.Invoke();
        }

        public void SetEyes(string id)
        {
            EyesID = id;
            OnChanged?.Invoke();
        }

        public void SetSkin(string id)
        {
            SkinID = id;
            OnChanged?.Invoke();
        }

        public void SetHairHSV(float h, float s, float v)
        {
            HairColor = ColorHelper.HSVToColor(h, s, v);
            OnChanged?.Invoke();
        }

        public void SetPantHSV(float h, float s, float v)
        {
            PantColor = ColorHelper.HSVToColor(h, s, v);
            OnChanged?.Invoke();
        }

        public void SetEyeHSV(float h, float s, float v)
        {
            EyeColor = ColorHelper.HSVToColor(h, s, v);
            OnChanged?.Invoke();
        }

        public void SetSkinHSV(float h, float s, float v)
        {
            SkinColor = ColorHelper.HSVToColor(h, s, v);
            OnChanged?.Invoke();
        }

        public void ApplyNormalizedSnapshot(
            PlayerAppearanceSnapshot snapshot,
            PlayerAppearanceSnapshot defaults)
        {
            IsCreated = snapshot.IsCreated;

            HairID = snapshot.HairID ?? defaults.HairID;
            GlassesID = snapshot.GlassesID ?? defaults.GlassesID;
            ShirtID = snapshot.ShirtID ?? defaults.ShirtID;
            PantID = snapshot.PantID ?? defaults.PantID;
            ShoeID = snapshot.ShoeID ?? defaults.ShoeID;
            EyesID = snapshot.EyesID ?? defaults.EyesID;
            SkinID = snapshot.SkinID ?? defaults.SkinID;

            HairColor = snapshot.HairColor == default ? defaults.HairColor : snapshot.HairColor;
            PantColor = snapshot.PantColor == default ? defaults.PantColor : snapshot.PantColor;
            EyeColor = snapshot.EyeColor == default ? defaults.EyeColor : snapshot.EyeColor;
            SkinColor = snapshot.SkinColor == default ? defaults.SkinColor : snapshot.SkinColor;
        }

        public void MarkCreated()
        {
            IsCreated = true;
        }
        #endregion
    }
}
