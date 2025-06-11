using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class TowerBuildSlotView : BaseSlot
    {
        public event Action<DragInformation> OnDragStart;
        
        public override void OnDropped(PointerEventData eventData)
        {
            /*if (RectTransformUtility.RectangleContainsScreenPoint(_dropArea, eventData.position,
                    eventData.pressEventCamera))
            {*/
            base.OnDropped(eventData);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _dropArea,
                    eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);

            if (eventData.pointerDrag.TryGetComponent(out RectTransform dropedRect) &&
                    eventData.pointerDrag.TryGetComponent(out DragDropElementView elementView))
            {
                if (_dropArea.CheckByEdgesFromPosition(localPoint, dropedRect.rect.width, dropedRect.rect.height))
                {
                    DragInformation dragInformation = new DragInformation(localPoint, _dropArea, elementView.DragSprite, dropedRect.rect.height);
                    OnDragStart?.Invoke(dragInformation);
                    Debug.Log($"<color=cyan>Right Drop at Local Position: {localPoint}</color>");
                }
                        
            }
            //}
        }
        
    }
}
