using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    public class HorizontalSelectorsLayout : MonoBehaviour
    {
        [SerializeField] protected RectTransform _panelRectTransform;
        [SerializeField] protected ScrollRect _scroll;
        [SerializeField] protected HorizontalLayoutGroup _layoutGroup;
        [SerializeField] protected ScrollRectFocusOnElement _focusOn;
        [SerializeField] protected int _minItemWidth = 300;

        protected List<RectTransform> _items;

        public void SetItems(IEnumerable<RectTransform> items)
        {
            _items = items.ToList();
            UpdateLayout();
        }

        protected void UpdateLayout()
        {
            if (_items.Count == 0)
                return;

            var calculatedCount = Mathf.Max(_items.Count, 2);
            var layoutWidth = _panelRectTransform.rect.width
                                - _layoutGroup.padding.left
                                - _layoutGroup.padding.right
                                - _layoutGroup.spacing * (calculatedCount - 1);

            var itemWidth = layoutWidth / calculatedCount;
            var enableScroll = false;

            if (itemWidth < _minItemWidth)
            {
                itemWidth = _minItemWidth;
                enableScroll = true;
            }

            _scroll.enabled = enableScroll;

            foreach (var rect in _items)
            {
                var size = rect.sizeDelta;
                size.x = itemWidth;
                rect.sizeDelta = size;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.transform as RectTransform);
        }

        public void FocusOn(int index)
        {
            if (_scroll.isActiveAndEnabled && index >= 0 && index < _items.Count)
                _focusOn.FocusOn(_items[index]);
        }
    }
}