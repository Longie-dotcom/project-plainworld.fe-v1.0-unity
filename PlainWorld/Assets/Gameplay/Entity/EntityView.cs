using System;
using UnityEngine;

public abstract class EntityView : MonoBehaviour
{
    #region Attributes
    #endregion

    #region Properties
    public Guid ID { get; private set; }
    #endregion

    #region Methods
    public virtual void Initialize(Guid id, Vector2 startPosition)
    {
        ID = id;
        transform.position = startPosition;
    }

    public abstract void ApplyPosition(Vector2 position);
    #endregion
}

