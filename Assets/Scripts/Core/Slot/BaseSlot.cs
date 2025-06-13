using DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Slot
{
    public abstract class BaseSlot : MonoBehaviour, IOnDropped
    {
        [SerializeField] protected RectTransform _dropArea;
        public virtual void OnDropped(PointerEventData eventData) { }

        protected bool InRectangle(PointerEventData eventData)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_dropArea, eventData.position,
                    eventData.pressEventCamera))
            {
                return true;
            }
            return false;
        }
    }
}