using UnityEngine;
using UnityEngine.Events;

namespace UAppToolkit.UIComponents
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasSizeChangedEvent : MonoBehaviour
    {
        public UnityEvent<Canvas> OnSizeChanged = new UnityEvent<Canvas>();
        private Canvas _canvas;

        private void OnEnable()
        {
            FindCanvas();
        }

        private void FindCanvas()
        {
            if (_canvas != null) return;
            _canvas = GetComponent<Canvas>();
        }

        private void OnRectTransformDimensionsChange()
        {
            FindCanvas();
            OnSizeChanged.Invoke(_canvas);
        }
    }
}