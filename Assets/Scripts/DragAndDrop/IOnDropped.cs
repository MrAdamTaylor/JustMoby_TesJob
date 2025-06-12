using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public interface IOnDropped
    {
        public void OnDropped(PointerEventData eventData);
    }
}