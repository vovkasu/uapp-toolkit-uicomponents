using UnityEngine;
using UnityEngine.UI;

namespace UAppToolKit.UIComponents.Extensions
{
    public static class RectTransformExtensions
    {
        public static Rect WorldRect(this RectTransform rectTransform)
        {
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);

            var min = new Vector2(float.MaxValue, float.MaxValue);
            var max = new Vector2(float.MinValue, float.MinValue);

            foreach (var corner in corners)
            {
                if (corner.x < min.x) min.x = corner.x;
                if (corner.y < min.y) min.y = corner.y;
                if (corner.x > max.x) max.x = corner.x;
                if (corner.y > max.y) max.y = corner.y;
            }

            return new Rect(min, max - min);
        }

        public static bool IsFullyInside(this RectTransform child, RectTransform parent)
        {
            var childCorners = new Vector3[4];
            child.GetWorldCorners(childCorners);

            var parentCorners = new Vector3[4];
            parent.GetWorldCorners(parentCorners);

            var parentMin = parentCorners[0];
            var parentMax = parentCorners[2];

            foreach (var corner in childCorners)
            {
                if (corner.x < parentMin.x || corner.x > parentMax.x || corner.y < parentMin.y || corner.y > parentMax.y)
                    return false;
            }

            return true;
        }

        public static RectTransform SetParentSize(this RectTransform rectTransform)
        {
            if (rectTransform == null)
                return rectTransform;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            return rectTransform;
        }

        public static void ScaleToParentSize(this RectTransform rectTransform)
        {
            if (rectTransform == null || !(rectTransform.parent is RectTransform parentRect))
                return;

            var rectSize = rectTransform.rect.size;
            var targetSize = parentRect.rect.size;

            // check GridLayoutGroup
            if (targetSize.x <= 0 || targetSize.y <= 0)
            {
                var gridLayout = parentRect.GetComponent<GridLayoutGroup>() ?? parentRect.parent?.GetComponent<GridLayoutGroup>();
                if (gridLayout != null)
                    targetSize = gridLayout.cellSize;
            }

            if (rectSize.x > 0 && rectSize.y > 0 && targetSize.x > 0 && targetSize.y > 0)
            {
                var scale = Mathf.Min(targetSize.x / rectSize.x, targetSize.y / rectSize.y);
                rectTransform.localScale = Vector3.one * Mathf.Max(scale, 0.001f);
            }
            else
            {
                rectTransform.localScale = Vector3.one;
            }
        }
    }
}