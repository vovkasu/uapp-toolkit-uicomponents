using System.Collections;
using Scroll.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MovingTower.UiComponents
{
    public class CarouselScrollSnap : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
    {
        private const float MinimumDuration = 0.0001f;

        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField, Min(0f)] private float _velocityThreshold = 10f;
        [SerializeField, Min(MinimumDuration)] private float _snapDuration = 0.35f;
        [SerializeField] private AnimationCurve _snapCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Coroutine _snapCoroutine;

        public void OnPointerDown(PointerEventData eventData)
        {
            StopSnap();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            StopSnap();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            StopSnap();
            _snapCoroutine = StartCoroutine(SnapToClosestOfferCoroutine());
        }

        private void OnDisable()
        {
            StopSnap();
        }

        private IEnumerator SnapToClosestOfferCoroutine()
        {
            var velocityThresholdSqr = _velocityThreshold * _velocityThreshold;
            while (_scrollRect.velocity.sqrMagnitude > velocityThresholdSqr)
                yield return null;

            var closestElement = GetClosestElement();
            if (closestElement != null)
            {
                _scrollRect.StopMovement();
                var duration = Mathf.Max(_snapDuration, MinimumDuration);
                yield return _scrollRect.FocusOnItemCoroutine(closestElement, duration, _snapCurve);
            }

            _snapCoroutine = null;
        }

        private RectTransform GetClosestElement()
        {
            var content = _scrollRect.content;
            if (content == null)
                return null;

            var viewport = _scrollRect.viewport != null
                ? _scrollRect.viewport
                : content.parent as RectTransform;

            if (viewport == null)
                return null;

            var viewportCenterX = viewport.TransformPoint(viewport.rect.center).x;
            var closestDistance = float.MaxValue;
            RectTransform closestElement = null;

            for (var i = 0; i < content.childCount; i++)
            {
                if (content.GetChild(i) is not RectTransform element || !element.gameObject.activeInHierarchy)
                    continue;

                var elementCenterX = element.TransformPoint(element.rect.center).x;
                var distance = Mathf.Abs(elementCenterX - viewportCenterX);
                if (distance >= closestDistance)
                    continue;

                closestDistance = distance;
                closestElement = element;
            }

            return closestElement;
        }

        private void StopSnap()
        {
            if (_snapCoroutine == null)
                return;

            StopCoroutine(_snapCoroutine);
            _snapCoroutine = null;
        }
    }
}
