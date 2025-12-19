using System;
using UnityEngine;

public class PlayerMoveView : MonoBehaviour
{
    #region Attributes
    #endregion

    #region Properties
    public event Action<Vector2> OnMove;
    #endregion

    public PlayerMoveView() { }

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
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"));

        if (dir != Vector2.zero)
            OnMove?.Invoke(dir);
    }

    public void ApplyPosition(Vector2 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, 0);
    }
    #endregion
}
