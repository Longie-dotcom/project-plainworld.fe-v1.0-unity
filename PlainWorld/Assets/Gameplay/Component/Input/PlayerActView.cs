using Assets.Data.Enum;
using Assets.State.Interface.State;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerActView : MonoBehaviour
{
    #region Attributes
    private const float DIR_EPSILON = 0.01f;

    private float moveSendTimer = 0f;
    private float moveSendRate;
    
    private Vector2 lastSentDir = Vector2.zero;
    private bool lastWasMoving = false;
    #endregion

    #region Properties
    public event Action<Vector2, EntityAction> OnUpdateVisualAction;
    public event Action OnSendActionToServer;
    #endregion

    #region Methods
    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {
        if (IsTyping())
            return;

        Vector2 dir = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        bool isAttack = Input.GetMouseButtonDown(0);
        bool isMoving = dir != Vector2.zero;

        // =========================
        // ATTACK (one-shot)
        // =========================
        if (isAttack)
        {
            // Visual prediction: attack, no movement
            OnUpdateVisualAction?.Invoke(Vector2.zero, EntityAction.ATTACK);

            // Send once
            OnSendActionToServer?.Invoke();
            return;
        }

        // =========================
        // MOVE / IDLE
        // =========================
        EntityAction action = isMoving ? EntityAction.RUN : EntityAction.IDLE;

        // Always predict locally
        OnUpdateVisualAction?.Invoke(dir, action);

        moveSendTimer += Time.deltaTime;

        bool directionChanged =
            isMoving &&
            Vector2.SqrMagnitude(dir - lastSentDir) > DIR_EPSILON * DIR_EPSILON;

        bool stateChanged =
            isMoving != lastWasMoving; // RUN <-> IDLE

        bool shouldSend =
            stateChanged ||
            (isMoving && moveSendTimer >= moveSendRate);

        if (shouldSend)
        {
            moveSendTimer = 0f;
            lastSentDir = dir;
            lastWasMoving = isMoving;

            OnSendActionToServer?.Invoke();
        }
    }

    public void ApplySettings(IReadOnlySettingState settings)
    {
        moveSendRate = settings.MoveSendRate;
    }

    private bool IsTyping()
    {
        if (EventSystem.current == null)
            return false;

        var selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null)
            return false;

        return selected.GetComponent<TMP_InputField>() != null;
    }
    #endregion
}
