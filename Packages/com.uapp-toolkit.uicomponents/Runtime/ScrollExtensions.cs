using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public static class ScrollExtensions
    {
        public static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, Vector2 focusPoint)
        {
            Vector2 contentSize = scrollView.content.rect.size;
            Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
            Vector2 contentScale = scrollView.content.localScale;

            contentSize.Scale(contentScale);
            focusPoint.Scale(contentScale);

            Vector2 scrollPosition = scrollView.normalizedPosition;
            if (scrollView.horizontal && contentSize.x > viewportSize.x)
                scrollPosition.x = Mathf.Clamp01((focusPoint.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
            if (scrollView.vertical && contentSize.y > viewportSize.y)
                scrollPosition.y = Mathf.Clamp01((focusPoint.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));

            return scrollPosition;
        }

        public static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, RectTransform item)
        {
            Vector2 itemCenterPoint = scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));

            Vector2 contentSizeOffset = scrollView.content.rect.size;
            contentSizeOffset.Scale(scrollView.content.pivot);

            return scrollView.CalculateFocusedScrollPosition(itemCenterPoint + contentSizeOffset);
        }

        public static void FocusAtPoint(this ScrollRect scrollView, Vector2 focusPoint)
        {
            scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(focusPoint);
        }

        public static void FocusOnItem(this ScrollRect scrollView, RectTransform item)
        {
            scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(item);
        }

        private static IEnumerator LerpToScrollPositionCoroutine(this ScrollRect scrollView, Vector2 targetNormalizedPos, float duration, AnimationCurve curve, Action onComplete = null)
        {
            if (duration <= 0.0001f)
            {
                Debug.LogWarning("Duration is too small. Setting scroll position immediately.");
                scrollView.normalizedPosition = targetNormalizedPos;
                onComplete?.Invoke();
                yield break;
            }

            Vector2 initialNormalizedPos = scrollView.normalizedPosition;

            var timer = 0f;
            while (timer < duration)
            {
                var t = timer / duration;
                if (curve != null)
                    t = curve.Evaluate(t);

                scrollView.normalizedPosition = Vector2.LerpUnclamped(initialNormalizedPos, targetNormalizedPos, t);

                yield return null;
                timer += Time.deltaTime;
            }

            scrollView.normalizedPosition = targetNormalizedPos;
            onComplete?.Invoke();
        }

        public static IEnumerator FocusAtPointCoroutine(this ScrollRect scrollView, Vector2 focusPoint, float duration, AnimationCurve curve = null)
        {
            yield return scrollView.LerpToScrollPositionCoroutine(scrollView.CalculateFocusedScrollPosition(focusPoint), duration, curve);
        }

        public static IEnumerator FocusOnItemCoroutine(this ScrollRect scrollView, RectTransform item, float duration, AnimationCurve curve = null)
        {
            yield return scrollView.LerpToScrollPositionCoroutine(scrollView.CalculateFocusedScrollPosition(item), duration, curve);
        }

        /// <summary>
        /// alignment: 0 - top/left, 0.5 - center, 1 - bottom/right
        /// </summary>
        public static void FocusOnItemAligned(this ScrollRect scrollView, RectTransform item, float alignment)
        {
            scrollView.normalizedPosition = scrollView.CalculateScrollToItemAligned(item, alignment);
        }

        public static IEnumerator FocusOnItemAlignedCoroutine(this ScrollRect scrollView, RectTransform item, float alignment, float duration, AnimationCurve curve = null, Action onComplete = null)
        {
            Vector2 target = scrollView.CalculateScrollToItemAligned(item, alignment);
            yield return scrollView.LerpToScrollPositionCoroutine(target, duration, curve, onComplete);
        }

        public static Vector2 CalculateScrollToItemAligned(this ScrollRect scrollView, RectTransform item, float alignment)
        {
            alignment = Mathf.Clamp01(alignment);

            var viewport = scrollView.viewport != null
                ? scrollView.viewport
                : (RectTransform)scrollView.content.parent;

            var contentSize = scrollView.content.rect.size;
            var viewportSize = viewport.rect.size;
            var itemLocalPos = scrollView.content.InverseTransformPoint(item.TransformPoint(item.rect.center));
            var normalizedPos = scrollView.normalizedPosition;

            if (scrollView.vertical && contentSize.y > viewportSize.y)
            {
                var contentHeight = contentSize.y;
                var viewportHeight = viewportSize.y;
                var itemTop = contentHeight * (1f - scrollView.content.pivot.y) - (itemLocalPos.y + item.rect.height * (1f - item.pivot.y));
                var targetY = itemTop - (viewportHeight - item.rect.height) * (1f - alignment);
                var normalizedY = 1f - Mathf.Clamp01(targetY / (contentHeight - viewportHeight));
                normalizedPos.y = normalizedY;
            }

            if (scrollView.horizontal && contentSize.x > viewportSize.x)
            {
                var contentWidth = contentSize.x;
                var viewportWidth = viewportSize.x;
                var itemLeft = itemLocalPos.x - item.rect.width * item.pivot.x;
                var targetX = itemLeft - (viewportWidth - item.rect.width) * (1f - alignment);
                var normalizedX = Mathf.Clamp01(targetX / (contentWidth - viewportWidth));
                normalizedPos.x = normalizedX;
            }

            return normalizedPos;
        }
    }
}