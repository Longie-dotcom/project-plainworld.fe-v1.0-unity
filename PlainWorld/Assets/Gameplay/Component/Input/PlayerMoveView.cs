using Assets.State.Interface.IReadOnlyState;
using Assets.Utility;
using System;
using UnityEngine;

public class PlayerMoveView : MonoBehaviour
{
    #region Attributes
    private const float DIR_EPSILON = 0.01f;

    private float moveSendTimer = 0f;
    private float moveSendRate;
    
    private Vector2 lastSentDir = Vector2.zero;
    private bool lastWasMoving = false;
    #endregion

    #region Properties
    public event Action<Vector2> OnUpdateVisualMove;
    public event Action OnSendMoveToServer;
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
        Vector2 dir = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        bool isMoving = dir != Vector2.zero;

        // Visual prediction (always)
        OnUpdateVisualMove?.Invoke(dir);

        moveSendTimer += Time.deltaTime;

        bool directionChanged =
            isMoving &&
            Vector2.SqrMagnitude(dir - lastSentDir) > DIR_EPSILON * DIR_EPSILON;

        bool stateChanged =
            isMoving != lastWasMoving; // RUN <-> IDLE

        bool shouldSend =
            stateChanged ||                 // send once on start/stop
            (isMoving && moveSendTimer >= moveSendRate); // throttle ONLY when moving

        if (shouldSend)
        {
            moveSendTimer = 0f;
            lastSentDir = dir;
            lastWasMoving = isMoving;

            OnSendMoveToServer?.Invoke();
        }
    }

    public void ApplySettings(IReadOnlySettingState settings)
    {
        moveSendRate = settings.MoveSendRate;
    }
    #endregion
}
