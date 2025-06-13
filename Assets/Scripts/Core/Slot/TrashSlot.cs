using System;
using DragAndDrop;
using EnterpriceLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Slot
{
    public class TrashSlot : BaseSlot
    {
        [SerializeField] private EllipseTrigger _ellipseTrigger;
    
        public event Action<DragInformation> OnDragStart;
    
        public override void OnDropped(PointerEventData eventData)
        {
            base.OnDropped(eventData);
        
            if(!InRectangle(eventData) && !_ellipseTrigger.CheckTrigger())
                return;
        
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _dropArea,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            if (eventData.pointerDrag.TryGetComponent(out RectTransform dropedRect) &&
                eventData.pointerDrag.TryGetComponent(out DragDropElementView elementView))
            {
                var dragInformation = new DragInformation(localPoint, _dropArea, elementView.QuadItem, dropedRect.rect.height, elementView);
                OnDragStart?.Invoke(dragInformation);
            }
        }
    }
}
