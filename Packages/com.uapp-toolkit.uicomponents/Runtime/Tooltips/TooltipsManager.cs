using System;
using System.Collections.Generic;
using UnityEngine;


namespace UAppToolkit.UIComponents.Tooltips
{
    public class TooltipsManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _root;
        [SerializeField] private TooltipViewBase _tooltipPrefab;

        private readonly Dictionary<TooltipViewBase, TooltipViewBase> _tooltipInstances = new();

        private RectTransform Root
        {
            get
            {
                if (!_root)
                    _root = (RectTransform)transform;
                
                return _root;
            }
        }


        private void Update()
            => HandleAnyMouseDown();

        public void SetDefaultTooltipPrefab(TooltipViewBase prefab)
            => _tooltipPrefab = prefab;

        public void ShowTooltip(Action<TooltipViewBase> setDataFunc, RectTransform target, TooltipSpawnType spawnType, Vector2 pivotOffset)
        {
            ShowTooltip(_tooltipPrefab, setDataFunc, target, spawnType, pivotOffset);
        }

        public void ShowTooltip(TooltipViewBase tooltipPrefab, Action<TooltipViewBase> setDataFunc, RectTransform target, TooltipSpawnType spawnType, Vector2 pivotOffset)
        {
            HideTooltips();

            if (!_tooltipInstances.ContainsKey(tooltipPrefab))
            {
                var tooltipInstance = Instantiate(tooltipPrefab, Root);
                _tooltipInstances.Add(tooltipPrefab, tooltipInstance);
            }

            InitTooltip(_tooltipInstances[tooltipPrefab], setDataFunc, target, spawnType, pivotOffset);
        }

        public void HideTooltips()
        {
            foreach (var tooltipInstance in _tooltipInstances.Values)
                tooltipInstance.gameObject.SetActive(false);
        }

        private void InitTooltip(TooltipViewBase tooltipInstance, Action<TooltipViewBase> setDataFunc, RectTransform target, TooltipSpawnType spawnType, Vector2 pivotOffset)
        {
            tooltipInstance.gameObject.SetActive(true);
            tooltipInstance.Init();
            setDataFunc?.Invoke(tooltipInstance);

            switch(spawnType)
            {
                case TooltipSpawnType.TargetPivot:
                    UpdateTooltipRectTransformValues(tooltipInstance, target, pivotOffset);
                    break;

                case TooltipSpawnType.Center:
                    tooltipInstance.transform.localPosition = Vector3.zero;
                    break;
            }
        }

        private void UpdateTooltipRectTransformValues(TooltipViewBase tooltipView, RectTransform target, Vector2 pivotOffset)
        {
            tooltipView.SetPosition(GetCenterOfRect(target));

            var tooltipRt = (RectTransform)tooltipView.transform;
            var screenBounds = GetBounds(Root);
            var targetBounds = GetBounds(target);
            var tooltipBounds = GetBounds(tooltipRt);

            var overflowCheck1 = RectInBounds.CalculateOffsetToStayInBounds(tooltipBounds, screenBounds);
            tooltipView.SetOverflowSettings(overflowCheck1);

            tooltipBounds = GetBounds(tooltipRt);
            tooltipBounds.center += GetRealPivotOffset(targetBounds, pivotOffset, overflowCheck1);

            var overflowCheck2 = RectInBounds.CalculateOffsetToStayInBounds(tooltipBounds, screenBounds);
            var finalPos = GetPositionWithPivot(tooltipBounds, tooltipRt.pivot) + overflowCheck2.RequiredOffset;
            tooltipView.SetPosition(finalPos);
        }

        private Vector3 GetRealPivotOffset(Bounds targetBounds, Vector2 pivotOffset, RectInBounds overflowCheck)
        {
            var flippedPivotOffset = pivotOffset;
            if (overflowCheck.RightOverflow)
                flippedPivotOffset.x = 1 - flippedPivotOffset.x;

            if (overflowCheck.TopOverflow)
                flippedPivotOffset.y = 1 - flippedPivotOffset.y;

            var realOffset = targetBounds.size;
            realOffset.Scale(flippedPivotOffset - Vector2.one * 0.5f);
            return realOffset;
        }

        private Vector3 GetPositionWithPivot(Bounds bounds, Vector2 pivot)
        {
            var scaledSize = bounds.size;
            scaledSize.Scale(pivot);
            return bounds.min + scaledSize;
        }

        private Bounds GetBounds(RectTransform rt)
        {
            var corners = GetCorners(rt);
            return new Bounds(GetCenterOfRect(rt), corners[2] - corners[0]);
        }

        private Vector3 GetCenterOfRect(RectTransform rt)
        {
            var corners = GetCorners(rt);
            var center = (corners[2] - corners[0]) / 2 + corners[0];
            return center;
        }

        private Vector3[] GetCorners(RectTransform rt)
        {
            var corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            return corners;
        }

        private void HandleAnyMouseDown()
        {
            if (IsTouchBegan() || Input.GetMouseButtonDown(0))
                HideTooltips();

            return;

            static bool IsTouchBegan()
            {
                return Input.touchSupported && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
            }
        }
    }
}