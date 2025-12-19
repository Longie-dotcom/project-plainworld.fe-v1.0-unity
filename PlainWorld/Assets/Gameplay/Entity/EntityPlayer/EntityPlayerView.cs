using UnityEngine;

public class EntityPlayerView : EntityView
{
    #region Attributes
    #endregion

    #region Properties
    #endregion

    public EntityPlayerView() : base() { }

    #region Methods
    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    public override void UpdatePosition(Vector2 position)
    {
        transform.position = position;
        // You could add player-specific animations or effects here
    }
    #endregion
}

