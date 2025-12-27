using System;
using UnityEngine;

public class PlayerMoveView : MonoBehaviour
{
    #region Attributes
    #endregion

    #region Properties
    public event Action<Vector2> OnMove;
    public event Action OnStop;
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

        if (dir != Vector2.zero)
            OnMove?.Invoke(dir.normalized);
        else
            OnStop?.Invoke();
    }
    #endregion
}
