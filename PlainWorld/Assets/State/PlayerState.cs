using System;
using UnityEngine;

namespace Assets.State
{
    public class PlayerState
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid PlayerID { get; private set; }
        public string PlayerName { get; private set; }
        public float MoveSpeed { get; private set; }

        public bool HasJoined { get; private set; }
        public Vector2 Position { get; private set; }

        public event Action<Vector2> OnPositionChanged;
        public event Action<bool> OnJoinChanged;
        public event Action OnCreatePlayer;
        #endregion

        public PlayerState()
        {
            MoveSpeed = 1f;
        }

        #region Methods
        public void MarkJoined(
            Guid playerId,
            string playerName)
        {
            if (HasJoined)
                return;

            PlayerID = playerId;
            PlayerName = playerName;
            HasJoined = true;

            OnJoinChanged?.Invoke(true);
        }

        public Vector2 PredictMove(Vector2 inputDir)
        {
            if (!HasJoined) return Position;

            if (inputDir == Vector2.zero)
                return Position;

            return Position + inputDir * MoveSpeed;
        }

        public void ApplyPredictedPosition(Vector2 position)
        {
            Position = position;
            OnPositionChanged?.Invoke(position);
        }

        public void ApplyServerPosition(Guid id, Vector2 position)
        {
            if (id != PlayerID) return;
            Position = position;
            OnPositionChanged?.Invoke(position);
        }

        public void LoadPlayerData(Vector2 position)
        {
            OnCreatePlayer?.Invoke();

            Position = position;
            OnPositionChanged?.Invoke(position);
        }
        #endregion
    }
}
