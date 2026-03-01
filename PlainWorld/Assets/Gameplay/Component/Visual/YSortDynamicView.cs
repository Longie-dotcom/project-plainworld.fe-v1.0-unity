using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Gameplay.Component.Visual
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class YSortDynamicView : MonoBehaviour
    {
        #region Attributes
        private SortingGroup group;
        private const int SORT_MULTIPLIER = 10; // EQUAL WITH THE FILE StaticObjectVisualView.cs
        #endregion

        #region Properties
        #endregion

        #region Methods
        void Awake()
        {
            group = GetComponent<SortingGroup>();
        }

        void Start()
        {
            
        }

        void Update()
        {
            
        }

        void LateUpdate()
        {
            group.sortingOrder =
                Mathf.RoundToInt(-transform.position.y * SORT_MULTIPLIER);
        }
        #endregion
    }
}