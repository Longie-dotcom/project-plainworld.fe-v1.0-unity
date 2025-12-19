using System;
using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    #region Attributes
    #endregion

    #region Properties
    public Guid Id { get; private set; }
    #endregion

    public EntityView() { }

    #region Methods
    public virtual void Initialize(Guid id, Vector2 startPosition)
    {
        Id = id;
        transform.position = startPosition;
    }

    public abstract void UpdatePosition(Vector2 position);
    #endregion
}

