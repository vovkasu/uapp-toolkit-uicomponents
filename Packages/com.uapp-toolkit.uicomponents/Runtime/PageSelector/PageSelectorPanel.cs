using System;
using System.Collections.Generic;
using UnityEngine;

namespace UAppToolkit.UIComponents.PageSelector
{
    public class PageSelectorPanel : HorizontalSelectorsLayout
    {
        [SerializeField] private PageSelector _pageSelectorPrefab;
        [SerializeField] private Transform _parentForItems;
        [SerializeField] private Transform _parentForOneItem;

        public int CurrentPageIndex { get; private set; }
        private Action<int> _onPageClick;
        private readonly List<PageSelector> _pageSelectors = new();
        
        public IReadOnlyList<PageSelector> Pages => _pageSelectors;

        public void Init(int pageCount, Action<int> onPageClick)
        {
            _onPageClick = onPageClick;
            var createdPages = SpawnPageSelectors(pageCount);

            _pageSelectors.Clear();
            _pageSelectors.AddRange(createdPages);

            var rects = createdPages.ConvertAll(p => p.RectTransform);
            SetItems(rects);
        }

        private List<PageSelector> SpawnPageSelectors(int count)
        {
            var pageSelectors = new List<PageSelector>();

            foreach (Transform child in _parentForItems)
                Destroy(child.gameObject);
            foreach (Transform child in _parentForOneItem)
                Destroy(child.gameObject);

            var parent = count == 1 ? _parentForOneItem : _parentForItems;
            for (int i = 0; i < count; i++)
            {
                var newPageSelector = Instantiate(_pageSelectorPrefab, parent);
                newPageSelector.Init(i, false, OnPageClick);
                pageSelectors.Add(newPageSelector);
            }

            return pageSelectors;
        }

        public void SetActive(int index)
        {
            CurrentPageIndex = index;

            for (var i = 0; i < _pageSelectors.Count; i++)
                _pageSelectors[i].SetActive(i == index);

            FocusOn(index);
        }

        private void OnPageClick(int index)
        {
            _onPageClick?.Invoke(index);
        }
    }
}