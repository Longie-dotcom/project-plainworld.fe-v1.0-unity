using Assets.State.Component.Player;
using Assets.State.Interface.IReadOnlyComponent.IReadOnlyPlayerComponent;
using Assets.State.Interface.IReadOnlyState;
using System;
using UnityEngine;

namespace Assets.State
{
    public class PlayerState : IReadOnlyPlayerState
    {
        #region Attributes
        private readonly PlayerMovement movement;
        private readonly PlayerAppearance appearance;
        #endregion

        #region Properties
        public Guid PlayerID { get; private set; }
        public string PlayerName { get; private set; }
        public bool HasJoined { get; private set; }

        public IReadOnlyPlayerMovement Movement { get { return movement; } }
        public IReadOnlyPlayerAppearance Appearance { get { return appearance; } }

        public event Action OnPlayerReady;
        public event Action OnPlayerLogout;
        public event Action OnPlayerForcedLogout;
        public event Action<PlayerAppearance> OnPlayerCustomization;
        #endregion

        public PlayerState()
        {
            movement = new PlayerMovement();
            appearance = new PlayerAppearance();
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
            this.appearance.ApplySnapshot(appearance);
            this.movement.ApplySnapshot(movement);

            // Load context
            OnPlayerReady?.Invoke();

            // Require customize character
            if (!this.appearance.IsCreated)
            {
                RequireCreateAppearance();
            }
        }

        public void Logout(Guid playerId)
        {
            if (!HasJoined || playerId != PlayerID) return;
            OnPlayerLogout?.Invoke();
            HasJoined = false;
        }

        public void ForcedLogout()
        {
            if (!HasJoined) return;
            OnPlayerForcedLogout?.Invoke();
            HasJoined = false;
        }
        #endregion

        #region Movement
        public bool TryCreateMovementCreation(
            out PlayerMovementSnapshot snapshot)
        {
            snapshot = default;

            if (!HasJoined)
                return false;

            snapshot = movement.CreateSnapshot();
            return true;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            if (!HasJoined) return;
            movement.SetMoveSpeed(moveSpeed);
        }

        public void ApplyPredictedPosition(Vector2 inputDir)
        {
            if (!HasJoined) return;
            movement.ApplyPredictedPosition(inputDir);
        }

        public void ApplyServerMovement(Guid id, PlayerMovementSnapshot snapshot)
        {
            if (!HasJoined || id != PlayerID) return;
            movement.ApplySnapshot(snapshot);
        }
        #endregion

        #region Appearance
        public bool TryPrepareAppearanceCreation(
            out PlayerAppearanceSnapshot snapshot)
        {
            snapshot = default;

            if (!HasJoined)
                return false;

            snapshot = appearance.PrepareForCreation();
            return true;
        }

        public void SetHair(string id)
        {
            if (!HasJoined) return;
            appearance.SetHair(id);
        }

        public void SetGlasses(string id)
        {
            if (!HasJoined) return;
            appearance.SetGlasses(id);
        }

        public void SetShirt(string id)
        {
            if (!HasJoined) return;
            appearance.SetShirt(id);
        }

        public void SetPant(string id)
        {
            if (!HasJoined) return;
            appearance.SetPant(id);
        }

        public void SetShoe(string id)
        {
            if (!HasJoined) return;
            appearance.SetShoe(id);
        }

        public void SetEyes(string id)
        {
            if (!HasJoined) return;
            appearance.SetEyes(id);
        }

        public void SetSkin(string id)
        {
            if (!HasJoined) return;
            appearance.SetSkin(id);
        }

        public void SetHairColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetHairHSV(h, s, v);
        }

        public void SetPantColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetPantHSV(h, s, v);
        }

        public void SetEyeColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetEyeHSV(h, s, v);
        }

        public void SetSkinColor(float h, float s, float v)
        {
            if (!HasJoined) return;
            appearance.SetSkinHSV(h, s, v);
        }

        public void ApplyServerAppearance(
            Guid id, 
            PlayerAppearanceSnapshot snapshot)
        {
            if (!HasJoined || id != PlayerID) return;
            appearance.ApplySnapshot(snapshot);
        }

        public void RequireCreateAppearance()
        {
            if (!HasJoined) return;
            OnPlayerCustomization?.Invoke(appearance);
        }

        public void NormalizeAppearance(
            PlayerAppearanceSnapshot snapshot,
            PlayerAppearanceSnapshot defaults)
        {
            if (!HasJoined) return;
            appearance.ApplyNormalizedSnapshot(snapshot, defaults);
        }
        #endregion
    }
}
