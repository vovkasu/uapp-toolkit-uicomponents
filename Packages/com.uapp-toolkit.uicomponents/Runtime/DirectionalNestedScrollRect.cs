using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MovingTower.UiComponents
{
    public class DirectionalNestedScrollRect : ScrollRect
    {
        private ScrollRect _parentScrollRect;
        private bool _isParentDrag;

        protected override void Awake()
        {
            base.Awake();
            _parentScrollRect = transform.parent != null
                ? transform.parent.GetComponentInParent<ScrollRect>()
                : null;
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            base.OnInitializePotentialDrag(eventData);
            _parentScrollRect?.OnInitializePotentialDrag(eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            var dragDelta = eventData.position - eventData.pressPosition;
            _isParentDrag = _parentScrollRect != null && ShouldDragParent(dragDelta);

            if (_isParentDrag)
            {
                StopMovement();
                _parentScrollRect.OnBeginDrag(eventData);
                return;
            }

            _parentScrollRect?.StopMovement();
            base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (_isParentDrag)
            {
                _parentScrollRect.OnDrag(eventData);
                return;
            }

            base.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (_isParentDrag)
                _parentScrollRect.OnEndDrag(eventData);
            else
                base.OnEndDrag(eventData);

            _isParentDrag = false;
        }

        private bool ShouldDragParent(Vector2 dragDelta)
        {
            var isHorizontalDrag = Mathf.Abs(dragDelta.x) > Mathf.Abs(dragDelta.y);
            var canScrollCurrent = isHorizontalDrag ? horizontal : vertical;
            var canScrollParent = isHorizontalDrag ? _parentScrollRect.horizontal : _parentScrollRect.vertical;

            return !canScrollCurrent && canScrollParent;
        }
    }
}
