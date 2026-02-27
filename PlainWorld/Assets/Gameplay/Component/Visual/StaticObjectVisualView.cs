using UnityEngine;

namespace Assets.Gameplay.Component.Visual
{
    [ExecuteAlways]
    public class StaticObjectVisualView : MonoBehaviour
    {
        #region Attributes
        private SpriteRenderer sr;

        private const float TILE_SIZE = 1f;   // 1 unit = 1 tile
        private const int SORT_MULTIPLIER = 10;
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
            if (Application.isPlaying)
            {
                ApplySorting();
                Destroy(this); // static → no runtime cost
            }
        }

        void OnValidate()
        {
            sr ??= GetComponent<SpriteRenderer>();
            SnapToGrid();
            ApplySorting();
        }

        private void ApplySorting()
        {
            if (sr == null) return;
            sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * SORT_MULTIPLIER);
        }

        private void SnapToGrid()
        {
            Vector3 p = transform.position;

            // X: center of tile
            p.x = Mathf.Floor(p.x / TILE_SIZE) * TILE_SIZE + TILE_SIZE * 0.5f;

            // Y: bottom of tile
            p.y = Mathf.Floor(p.y / TILE_SIZE) * TILE_SIZE + TILE_SIZE * 0.5f;

            transform.position = p;
        }
        #endregion
    }
}
