using System;
using UnityEngine;

public class PlayerMoveView : MonoBehaviour
{
    #region Attributes
    private float moveSendTimer = 0f;
    private float moveSendRate = 0.01f; // 100 times per second
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
            Input.GetAxisRaw("Vertical"));

        // Update and predict visuals immediately
        OnUpdateVisualMove?.Invoke(dir.normalized);

        // Throttle sending predicted moves to the server
        moveSendTimer += Time.deltaTime;
        if (moveSendTimer >= moveSendRate)
        {
            moveSendTimer = 0f;
            OnSendMoveToServer?.Invoke();
        }
    }
    #endregion
}
