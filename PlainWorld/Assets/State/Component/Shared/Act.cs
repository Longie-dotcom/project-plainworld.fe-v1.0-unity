using Assets.Data.Enum;
using Assets.State.Interface.Component.Shared;
using System;
using UnityEngine;

namespace Assets.State.Component.Shared
{
    public readonly struct ActSnapshot
    {
        public readonly float MoveSpeed;
        public readonly Vector2 Position;
        public readonly Vector2 CurrentDirection;
        public readonly EntityAction CurrentAction;

        public ActSnapshot(
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

    public class Act : IReadOnlyAct
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
        public event Action OnItemUsed;
        #endregion

        public Act()
        {
            MoveSpeed = 5f;
            CurrentAction = EntityAction.IDLE;
            CurrentDirection = Vector2.down;
        }

        #region Methods
        public void ApplySnapshot(ActSnapshot snapshot)
        {
            MoveSpeed = snapshot.MoveSpeed <= 0f ? 5f : snapshot.MoveSpeed;
            Position = snapshot.Position;
            CurrentDirection = snapshot.CurrentDirection;
            CurrentAction = snapshot.CurrentAction;

            // Notify listeners
            OnItemUsed?.Invoke();
            OnMoveSpeedChanged?.Invoke(MoveSpeed);
            OnPositionChanged?.Invoke(Position);
            OnDirectionChanged?.Invoke(CurrentDirection);
            OnActionChanged?.Invoke(CurrentAction);
        }

        public (Vector2 direction, int action) TryCreateActionCreation()
        {
            return (CurrentDirection, (int)CurrentAction);
        }

        public void ApplyPredictedAction(
            Vector2 inputDir,
            EntityAction action)
        {
            // =========================
            // ATTACK: no movement prediction
            // =========================
            if (action == EntityAction.ATTACK)
            {
                OnItemUsed?.Invoke();
                SetCurrentAction(EntityAction.ATTACK);
                return;
            }

            // =========================
            // MOVEMENT
            // =========================
            if (inputDir != Vector2.zero && action == EntityAction.RUN)
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

        public void SetMoveSpeed(float moveSpeed)
        {
            if (MoveSpeed != moveSpeed)
            {
                MoveSpeed = moveSpeed;
                OnMoveSpeedChanged?.Invoke(moveSpeed);
            }
        }

        public void SetPosition(Vector2 position)
        {
            if (Position != position)
            {
                Position = position;
                OnPositionChanged?.Invoke(position);
            }
        }

        public void SetCurrentDirection(Vector2 direction)
        {
            if (CurrentDirection != direction)
            {
                CurrentDirection = direction;
                OnDirectionChanged?.Invoke(direction);
            }
        }

        public void SetCurrentAction(EntityAction action)
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
