using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class TowerBuildSlotView : MonoBehaviour, IDropHandler
    {
        public event Action<DragInformation> OnDragStart;
        
        [SerializeField] private RectTransform _dropArea;
        public void OnDrop(PointerEventData eventData)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(_dropArea, eventData.position,
                    eventData.pressEventCamera))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _dropArea,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPoint
                );

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
            }

        }
        
    }
}
