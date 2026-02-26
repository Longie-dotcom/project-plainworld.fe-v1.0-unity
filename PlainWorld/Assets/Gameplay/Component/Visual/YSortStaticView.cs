using UnityEngine;

namespace Assets.Gameplay.Component.Visual
{
    public class YSortStaticView : MonoBehaviour
    {
        #region Attributes
        private SpriteRenderer sr;
        #endregion

        #region Properties
        #endregion

        #region Methods
        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
            Destroy(this); // remove YSort script after setting once
        }

        void Update()
        {

        }
        #endregion
    }
}
