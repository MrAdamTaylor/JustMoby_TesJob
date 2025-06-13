using System;
using DragAndDrop;
using Extension;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class TowerBuildSlotView : BaseSlot
    {
        public event Action<DragInformation> OnDragStart;
        
        public override void OnDropped(PointerEventData eventData)
        {
            base.OnDropped(eventData);
        
            if(!InRectangle(eventData))
                return;
            
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
                    DragInformation dragInformation = new DragInformation(localPoint, _dropArea, elementView.QuadItem, dropedRect.rect.height, elementView);
                    OnDragStart?.Invoke(dragInformation);
                }
            }
        }
        
    }
}
