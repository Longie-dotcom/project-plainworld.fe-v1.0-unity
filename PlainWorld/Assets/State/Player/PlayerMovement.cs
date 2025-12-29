using Assets.Data.Enum;
using System;
using UnityEngine;

namespace Assets.State.Player
{
    public readonly struct PlayerMovementSnapshot
    {
        public readonly float MoveSpeed;
        public readonly Vector2 Position;
        public readonly Vector2 CurrentDirection;
        public readonly EntityAction CurrentAction;

        public PlayerMovementSnapshot(
            float moveSpeed,
            Vector2 position,
            Vector2 currentDirection,
            EntityAction currentAction)
        {
            MoveSpeed = moveSpeed;
            Position = position;
            CurrentDirection = currentDirection;
            CurrentAction = currentAction;
        }
    }

    public class PlayerMovement
    {
        #region Attributes
        #endregion

        #region Properties
        public float MoveSpeed { get; private set; }
        public Vector2 Position { get; private set; }
        public Vector2 CurrentDirection { get; private set; }
        public EntityAction CurrentAction { get; private set; }

        public event Action<float> OnMoveSpeedChanged;
        public event Action<Vector2> OnPositionChanged;
        public event Action<Vector2> OnDirectionChanged;
        public event Action<EntityAction> OnActionChanged;
        #endregion

        public PlayerMovement()
        {
            MoveSpeed = 5f;
            CurrentAction = EntityAction.IDLE;
            CurrentDirection = Vector2.down;
        }

        #region Methods
        internal void ApplySnapshot(PlayerMovementSnapshot snapshot)
        {
            MoveSpeed = snapshot.MoveSpeed <= 0f ? 5f : snapshot.MoveSpeed;
            Position = snapshot.Position;
            CurrentDirection = snapshot.CurrentDirection;
            CurrentAction = snapshot.CurrentAction;

            // Notify listeners
            OnMoveSpeedChanged?.Invoke(MoveSpeed);
            OnPositionChanged?.Invoke(Position);
            OnDirectionChanged?.Invoke(CurrentDirection);
            OnActionChanged?.Invoke(CurrentAction);
        }

        internal PlayerMovementSnapshot CreateSnapshot()
        {
            return new PlayerMovementSnapshot(
                MoveSpeed,
                Position,
                CurrentDirection,
                CurrentAction
            );
        }

        internal void ApplyPredictedPosition(Vector2 inputDir)
        {
            if (inputDir != Vector2.zero)
            {
                Position += inputDir * MoveSpeed * Time.deltaTime;

                SetPosition(Position);
                SetCurrentDirection(inputDir);
                SetCurrentAction(EntityAction.RUN);
            }
            else
            {
                SetCurrentAction(EntityAction.IDLE);
            }
        }

        internal void SetMoveSpeed(float moveSpeed)
        {
            if (MoveSpeed != moveSpeed)
            {
                MoveSpeed = moveSpeed;
                OnMoveSpeedChanged?.Invoke(moveSpeed);
            }
        }

        internal void SetPosition(Vector2 position)
        {
            if (Position != position)
            {
                Position = position;
                OnPositionChanged?.Invoke(position);
            }
        }

        internal void SetCurrentDirection(Vector2 direction)
        {
            if (CurrentDirection != direction)
            {
                CurrentDirection = direction;
                OnDirectionChanged?.Invoke(direction);
            }
        }

        internal void SetCurrentAction(EntityAction action)
        {
            if (CurrentAction != action)
            {
                CurrentAction = action;
                OnActionChanged?.Invoke(action);
            }
        }
        #endregion
    }
}
