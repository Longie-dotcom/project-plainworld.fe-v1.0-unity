using UnityEngine;

namespace Assets.Gameplay.Component.Visual
{
    public class CameraBounds : MonoBehaviour
    {
        public Vector2 min;
        public Vector2 max;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                (min + max) * 0.5f,
                max - min
            );
        }
    }
}
