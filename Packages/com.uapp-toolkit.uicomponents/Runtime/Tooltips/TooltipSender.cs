using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UAppToolkit.UIComponents.Tooltips
{
    public class TooltipSender : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TooltipViewBase _customTooltipPrefab;
        [SerializeField] private TooltipEventType _tooltipEventType = TooltipEventType.Click;
        [SerializeField] private TooltipSpawnType _tooltipSpawnType = TooltipSpawnType.TargetPivot;
        [SerializeField, ShowIf(nameof(_tooltipSpawnType), TooltipSpawnType.TargetPivot)]
        private Vector2 _spawnPivotOffset = new Vector2(0.5f, 1f);

        private TooltipsManager _tooltipsManager;
        private bool _isTooltipsManagerGot;

        private Selectable _selectable;
        private Graphic _graphic;
        private bool _isComponentsGot;

        private Action<TooltipViewBase> _setDataFunc;
        public TooltipEventType TooltipEventType => _tooltipEventType;

        public void SetTooltip(Action<TooltipViewBase> initFunc)
        {
            SetTooltip(initFunc, _tooltipEventType);
        }

        public void SetTooltip(Action<TooltipViewBase> setDataFunc, TooltipEventType tooltipEventType)
        {
            _tooltipEventType = tooltipEventType;
            _setDataFunc = setDataFunc;

            TryGetComponents();
            SubscribeEvents();
        }

        public void RemoveTooltip()
        {
            UnsubscribeEvents();

            _isComponentsGot = false;
            _selectable = null;
            _graphic = null;
            _setDataFunc = null;
            _tooltipEventType = TooltipEventType.None;
        }

        public void ShowTooltip()
        {
            try
            {
                if (_customTooltipPrefab)
                    _tooltipsManager?.ShowTooltip(_customTooltipPrefab, _setDataFunc, (RectTransform)transform, _tooltipSpawnType, _spawnPivotOffset);
                else
                    _tooltipsManager?.ShowTooltip(_setDataFunc, (RectTransform)transform, _tooltipSpawnType, _spawnPivotOffset);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            TryHandleNotInteractableClick();
            TryHandleNotSelectableClick();
        }

        private void SubscribeEvents()
        {
            UnsubscribeEvents();

            if (_tooltipEventType == TooltipEventType.LongPress)
            {
                if (_selectable is ButtonLongPress longPressBtn)
                {
                    longPressBtn.OnLongPressAction.AddListener(ShowTooltip);
                    return;
                }

                Debug.LogError($"{nameof(TooltipSender)}: {nameof(TooltipEventType.LongPress)} event type is selected but {nameof(ButtonLongPress)} component is not found", this);
                return;
            }

            if (_tooltipEventType == TooltipEventType.Click)
            {
                if (_selectable is Button button)
                {
                    button.onClick.AddListener(ShowTooltip);
                    return;
                }

                if (_graphic && _graphic.raycastTarget)
                    return;

                Debug.LogError($"{nameof(TooltipSender)}: {nameof(TooltipEventType.Click)} event type is selected but there is no {nameof(Button)} component or raycast target", this);
                return;
            }
        }
        
        private void UnsubscribeEvents()
        {
            if (_selectable is ButtonLongPress longPressBtn)
                longPressBtn.OnLongPressAction.RemoveListener(ShowTooltip);

            if (_selectable is Button button)
                button.onClick.RemoveListener(ShowTooltip);
        }

        private void TryGetComponents()
        {
            if (!_isComponentsGot)
            {
                _selectable = GetComponent<Selectable>();
                _graphic = GetComponent<Graphic>();

                _isComponentsGot = true;
            }

            if (!_isTooltipsManagerGot)
            {
                _tooltipsManager = GetComponentInParent<TooltipsManager>(true);
                if (!_tooltipsManager)
                    Debug.LogError($"{nameof(TooltipSender)}: {nameof(TooltipsManager)} component is not found in parent objects");

                _isTooltipsManagerGot = true;
            }
        }

        private void TryHandleNotInteractableClick()
        {
            if (_tooltipEventType != TooltipEventType.NotInteractableClick)
                return;

            if (!_selectable || _selectable.interactable)
                return;

            ShowTooltip();
        }

        private void TryHandleNotSelectableClick()
        {
            if (_tooltipEventType != TooltipEventType.Click)
                return;

            if (_selectable)
                return;

            ShowTooltip();
        }
    }
}