using Core.Tower;
using DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Slot
{
    public class TowerUpSlot : BaseSlot
    {
        public RectTransform DropArea => _dropArea;
    
        private QuadTower _quadTower;

        public void SetTown(QuadTower quadTower)
        {
            _quadTower = quadTower;
        }

        public override void OnDropped(PointerEventData eventData)
        {
            base.OnDropped(eventData);
        
            if(!InRectangle(eventData))
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
                _quadTower.AddToTown(dragInformation);
            }
        }
    }
}