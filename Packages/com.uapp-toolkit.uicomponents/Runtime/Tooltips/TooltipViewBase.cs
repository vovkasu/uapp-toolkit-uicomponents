using UnityEngine;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents.Tooltips
{
    public abstract class TooltipViewBase : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _imgBackgroundRoot;
        [SerializeField] private Image _imgBackground;

        private Vector2 _defaultPivot;
        private Vector2 _defaultImageScale;
        private bool _isInitialized;
        private static readonly int ShowTriggerKey = Animator.StringToHash("Show");

        public void Init()
        {
            _animator?.SetTrigger(ShowTriggerKey);
            if (_isInitialized)
            {
                SetDefaultValues();
                return;
            }

            _defaultPivot = _rectTransform.pivot;
            _defaultImageScale = _imgBackground.rectTransform.localScale;
            _isInitialized = true;
        }

        public void SetPosition(Vector3 position)
        {
            _rectTransform.position = position;
        }

        public void SetOverflowSettings(RectInBounds rectInScreenBounds)
        {
            SetPivotByOverflowSettings(rectInScreenBounds);
            SetImageScalesByOverflowSettings(rectInScreenBounds);
        }

        private void SetDefaultValues()
        {
            _rectTransform.pivot = _defaultPivot;
            _imgBackground.rectTransform.localScale = _defaultImageScale;
        }

        private void SetPivotByOverflowSettings(RectInBounds rectInBounds)
        {
            var pivot = _defaultPivot;
            var newPivot = _rectTransform.pivot;

            if (rectInBounds.TopOverflow)
            {
                newPivot.y = 1.0f - pivot.y;
            }

            if (rectInBounds.RightOverflow)
            {
                newPivot.x = 1.0f - pivot.x;
            }

            _rectTransform.pivot = newPivot;
            _imgBackgroundRoot.pivot = newPivot;
        }

        private void SetImageScalesByOverflowSettings(RectInBounds rectInBounds)
        {
            var newScale = Vector2.one;
            if (rectInBounds.TopOverflow)
            {
                newScale.y *= -1;
            }

            if (rectInBounds.RightOverflow)
            {
                newScale.x *= -1;
            }

            _imgBackground.rectTransform.localScale = newScale;
        }
    }
}