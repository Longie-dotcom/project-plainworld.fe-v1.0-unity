using Assets.Data.Enum;
using Assets.Utility;
using System;
using UnityEngine;

namespace Assets.State.Player
{
    public class PlayerState
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid PlayerID { get; private set; }
        public string PlayerName { get; private set; }
        public bool HasJoined { get; private set; }

        public PlayerMovement Movement { get; }
        public PlayerAppearance Appearance { get; }

        public event Action OnPlayerReady;
        public event Action OnPlayerNeedsCustomization;
        public event Action<PlayerAppearance> OnAppearanceLoaded;
        #endregion

        public PlayerState()
        {
            Movement = new PlayerMovement();
            Appearance = new PlayerAppearance();
        }

        #region Methods
        public void LoadPlayerData(
            Guid playerId,
            string playerName,
            PlayerMovementSnapshot movement,
            PlayerAppearanceSnapshot appearance)
        {
            if (HasJoined) return;

            // Load identity
            PlayerID = playerId;
            PlayerName = playerName;
            HasJoined = true;
            Appearance.LoadFromSnapshot(appearance);

            // Domain flow decision
            if (!appearance.IsCreated)
            {
                OnPlayerNeedsCustomization?.Invoke();
                OnAppearanceLoaded?.Invoke(Appearance);
            }
            else
            {
                OnPlayerReady?.Invoke();
            }

            // Load context
            Movement.ApplySnapshot(movement);
        }

        public void ConfirmAppearanceCreated()
        {
            if (!HasJoined)
                return;

            if (Appearance.IsCreated)
                return;

            Appearance.MarkCreated();

            OnPlayerReady?.Invoke();
        }
        #endregion

        #region Movement
        public bool TryCreateMovementCreation(
            out PlayerMovementSnapshot snapshot)
        {
            snapshot = default;

            if (!HasJoined)
                return false;

            snapshot = Movement.CreateSnapshot();
            return true;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            if (!HasJoined) return;
            Movement.SetMoveSpeed(moveSpeed);
        }

        public void ApplyPredictedPosition(Vector2 inputDir)
        {
            if (!HasJoined) return;
            Movement.ApplyPredictedPosition(inputDir);
        }

        public void ApplyServerPosition(Guid id, PlayerMovementSnapshot snapshot)
        {
            if (!HasJoined || id != PlayerID) return;
            Movement.ApplySnapshot(snapshot);
        }
        #endregion

        #region Appearance
        public bool TryPrepareAppearanceCreation(
            out PlayerAppearanceSnapshot snapshot)
        {
            snapshot = default;

            if (!HasJoined)
                return false;

            snapshot = Appearance.PrepareForCreation();
            return true;
        }

        public void SetHair(string id)
        {
            if (!HasJoined) return;
            Appearance.SetHair(id);
        }

        public void SetGlasses(string id)
        {
            if (!HasJoined) return;
            Appearance.SetGlasses(id);
        }

        public void SetShirt(string id)
        {
            if (!HasJoined) return;
            Appearance.SetShirt(id);
        }

        public void SetPant(string id)
        {
            if (!HasJoined) return;
            Appearance.SetPant(id);
        }

        public void SetShoe(string id)
        {
            if (!HasJoined) return;
            Appearance.SetShoe(id);
        }

        public void SetEyes(string id)
        {
            if (!HasJoined) return;
            Appearance.SetEyes(id);
        }

        public void SetSkin(string id)
        {
            if (!HasJoined) return;
            Appearance.SetSkin(id);
        }

        public void SetHairColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            Appearance.SetHairHSV(h, s, v);
        }

        public void SetPantColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            Appearance.SetPantHSV(h, s, v);
        }

        public void SetEyeColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            Appearance.SetEyeHSV(h, s, v);
        }

        public void SetSkinColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            Appearance.SetSkinHSV(h, s, v);
        }
        #endregion
    }
}
