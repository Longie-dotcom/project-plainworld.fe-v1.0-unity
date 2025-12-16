using UnityEngine;

public class PlayerView : MonoBehaviour
{
    #region Attributes
    private ViewPresenter logic;
    #endregion

    #region Properties
    #endregion

    #region Methods
    void Update()
    {
        if (logic == null) return;

        if (!logic.HasJoined && Input.GetKeyDown(KeyCode.J))
            _ = logic.Join();

        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (dir != Vector2.zero)
            _ = logic.Move(dir);
    }

    public void SetLogic(ViewPresenter logic)
    {
        this.logic = logic;
    }
    #endregion
}
