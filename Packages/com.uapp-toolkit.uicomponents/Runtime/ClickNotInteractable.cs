using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UAppToolkit.UIComponents
{
    [RequireComponent(typeof(Selectable))]
    public class ClickNotInteractable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
    {
        public UnityEvent OnNotInteractableClick;
        public bool IsEnable = true;
        private Selectable _selectable;
        private bool? _selectableInteractableOnPointerDown;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (_selectable || TryGetComponent(out _selectable))
            {
                _selectableInteractableOnPointerDown = _selectable.interactable;
                return;
            }

            Debug.LogError("Selectable component not found", this);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (_selectableInteractableOnPointerDown.HasValue && !_selectableInteractableOnPointerDown.Value)
            {
                if (IsEnable)
                {
                    OnNotInteractableClick?.Invoke();
                }
            }

            _selectableInteractableOnPointerDown = null;
        }
    }
}