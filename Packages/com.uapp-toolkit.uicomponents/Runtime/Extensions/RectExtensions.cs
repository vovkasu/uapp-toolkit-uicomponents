using UnityEngine;

namespace UAppToolKit.UIComponents.Extensions
{
    public static class RectExtensions
    {
        public static bool Contains(this Rect baseRect, Rect otherRect)
        {
            return baseRect.Contains(otherRect.min) && baseRect.Contains(otherRect.max);
        }
    }
}