using UnityEngine;

/*
 * Color space conversion helper.
 *
 * Purpose:
 * - Convert HSV values to Unity Color.
 * - Extract HSV components from Unity Color.
 *
 * Usage:
 * - Used for appearance customization and network-safe color storage.
 *
 * Notes:
 * - HSV is preferred for user-facing color editing.
 * - Unity Color remains the rendering format.
 */

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
