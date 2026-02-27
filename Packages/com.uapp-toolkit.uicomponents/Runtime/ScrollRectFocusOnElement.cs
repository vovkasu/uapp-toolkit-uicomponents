using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UAppToolkit.UIComponents
{
    public class ScrollRectFocusOnElement : MonoBehaviour
    {
        [SerializeField] public ScrollRect _scrollRect;
        [SerializeField] private float _focusDuration = 0.5f;
        [SerializeField] private float _focusPositionThreshold = 0.01f;
        [SerializeField] private AnimationCurve _focusCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private Coroutine focusOnCoroutine;
        private Action _onFocusComplete;

        public RectTransform ContentTransform => _scrollRect.content;

        public void FocusOn(RectTransform target, bool withAnimation = true, float alignment = 0.5f, Action onComplete = null)
        {
            if (focusOnCoroutine != null)
            {
                StopCoroutine(focusOnCoroutine);
                focusOnCoroutine = null;
            }

            _onFocusComplete = onComplete;
            if (withAnimation && !IsInFocus(target, alignment))
            {
                focusOnCoroutine = StartCoroutine(FocusOnItemAlignedCoroutine(target, alignment));
            }
            else
            {
                _scrollRect.FocusOnItemAligned(target, alignment);
                _onFocusComplete?.Invoke();
            }
        }

        private IEnumerator FocusOnItemAlignedCoroutine(RectTransform target, float alignment)
        {
            yield return _scrollRect.FocusOnItemAlignedCoroutine(target, alignment, _focusDuration, _focusCurve);
            _onFocusComplete?.Invoke();
        }

        private bool IsInFocus(RectTransform target, float alignment)
        {
            var currentPos = _scrollRect.normalizedPosition;
            var targetPos = _scrollRect.CalculateScrollToItemAligned(target, alignment);
            return Vector2.Distance(currentPos, targetPos) <= _focusPositionThreshold;
        }
    }
}