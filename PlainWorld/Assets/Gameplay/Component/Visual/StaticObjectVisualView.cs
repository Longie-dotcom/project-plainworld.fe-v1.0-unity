using System;
using UnityEngine;

namespace Assets.Gameplay.Component.Visual
{
    public struct CollisionBox
    {
        public Vector2 Min;
        public Vector2 Max;

        public CollisionBox(Vector2 min, Vector2 max)
        {
            Min = min;
            Max = max;
        }

        public override string ToString()
        {
            return $"Min({Min.x:F2}, {Min.y:F2}) Max({Max.x:F2}, {Max.y:F2})";
        }
    }

    [ExecuteAlways]
    [RequireComponent(typeof(SpriteRenderer))]
    public class StaticObjectVisualView : MonoBehaviour
    {
        #region Attributes
        private SpriteRenderer sr;

        private const float TILE_SIZE = 1f;   // 1 unit = 1 tile
        private const int SORT_MULTIPLIER = 10;
        #endregion

        #region Properties
        [ContextMenu("Print Current Collision Box")]
        private void PrintCurrentCollisionBox()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col == null)
            {
                Debug.Log($"{name} has no Collider2D");
                return;
            }

            Bounds b = col.bounds; // WORLD SPACE
            Debug.Log($"Collision Box for {name} => Min({b.min.x:F2}, {b.min.y:F2}) Max({b.max.x:F2}, {b.max.y:F2})");
        }
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
                //Destroy(this); // static → no runtime cost
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

        public CollisionBox GetWorldCollisionBox()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col == null)
                throw new Exception($"{name} has no Collider2D");

            Bounds b = col.bounds; // WORLD SPACE

            return new CollisionBox(
                new Vector2(b.min.x, b.min.y),
                new Vector2(b.max.x, b.max.y)
            );
        }
        #endregion
    }
}
