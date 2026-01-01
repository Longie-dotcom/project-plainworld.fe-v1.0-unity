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
            if (!s.IsCreated) return;

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

        public void EnsureDefaults(PlayerAppearanceSnapshot s)
        {
            bool changed = false;

            if (string.IsNullOrEmpty(HairID)) 
            { 
                HairID = s.HairID; 
                changed = true; 
            }

            if (string.IsNullOrEmpty(GlassesID)) 
            { 
                GlassesID = s.GlassesID; 
                changed = true; 
            }

            if (string.IsNullOrEmpty(ShirtID)) 
            { 
                ShirtID = s.ShirtID;
                changed = true; 
            }

            if (string.IsNullOrEmpty(PantID)) 
            { 
                PantID = s.PantID; 
                changed = true; 
            }

            if (string.IsNullOrEmpty(ShoeID)) 
            { 
                ShoeID = s.ShoeID; 
                changed = true;
            }

            if (string.IsNullOrEmpty(EyesID)) 
            { 
                EyesID = s.EyesID; 
                changed = true; 
            }

            if (string.IsNullOrEmpty(SkinID)) 
            { 
                SkinID = s.SkinID; 
                changed = true; 
            }

            if (HairColor.a == 0f) 
            { 
                HairColor = s.HairColor; 
                changed = true; 
            }

            if (PantColor.a == 0f) 
            { 
                PantColor = s.PantColor;
                changed = true; 
            }

            if (EyeColor.a == 0f) 
            { 
                EyeColor = s.EyeColor; 
                changed = true; 
            }

            if (SkinColor.a == 0f) 
            { 
                SkinColor = s.SkinColor; 
                changed = true; 
            }

            if (changed)
                OnChanged?.Invoke();
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

        public void MarkCreated()
        {
            IsCreated = true;
        }
        #endregion
    }
}
