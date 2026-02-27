using UnityEngine;
using UnityEngine.EventSystems;

namespace UAppToolkit.UIComponents
{
    public class DragEventInterrupter : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            eventData.Use();
        }

        public void OnDrag(PointerEventData eventData)
        {
            eventData.Use();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            eventData.Use();
        }
    }
}