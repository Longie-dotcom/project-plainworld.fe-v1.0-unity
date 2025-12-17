using System;
using UnityEngine;

namespace Assets.State
{
    public class PlayerState
    {
        #region Attributes
        #endregion

        #region Properties
        public Guid PlayerId { get; }
        public string PlayerName { get; }
        public float MoveSpeed { get; } = 5f;

        public bool HasJoined { get; private set; }
        public Vector2 Position { get; private set; }

        public event Action<Vector2> OnPositionChanged;
        public event Action<bool> OnJoinChanged;
        #endregion

        public PlayerState()
        {
            PlayerId = Guid.Parse("a5a5405a-c1e1-49af-a68c-7cbb035be75d");
            PlayerName = "Long";
        }

        #region Methods
        public void MarkJoined()
        {
            if (HasJoined)
                return;

            HasJoined = true;
            OnJoinChanged?.Invoke(true);
        }

        public Vector2 PredictMove(Vector2 inputDir)
        {
            if (inputDir == Vector2.zero)
                return Position;

            return Position + inputDir * MoveSpeed;
        }

        public void ApplyPredictedPosition(Vector2 position)
        {
            Position = position;
            OnPositionChanged?.Invoke(position);
        }

        public void ApplyServerPosition(Vector2 position)
        {
            Position = position;
            OnPositionChanged?.Invoke(position);
        }
        #endregion
    }
}
