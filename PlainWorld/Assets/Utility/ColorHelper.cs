using UnityEngine;

namespace Assets.Utility
{
    public static class ColorHelper
    {
        public static Color HSVToColor(float h, float s, float v)
        {
            return Color.HSVToRGB(h, s, v);
        }

        public static (float h, float s, float v) ColorToHSV(Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return (h, s, v);
        }
    }
}
