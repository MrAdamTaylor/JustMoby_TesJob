using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDrop
{
    public class TownBuildSlot : MonoBehaviour, IDropHandler
    {
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
                
                RectTransform dropedRect = eventData.pointerDrag.GetComponent<RectTransform>();

                if (_dropArea.CheckByEdgesFromPosition(localPoint, dropedRect.rect.width, dropedRect.rect.height))
                {
                    Debug.Log($"<color=cyan>Right Drop at Local Position: {localPoint}</color>");
                }
                /*Debug.Log($"Левый край {_dropArea.rect.xMin}, Правый край {_dropArea.rect.xMax}, " +
                          $"Верхний край {_dropArea.rect.yMax}, Нижний край {_dropArea.rect.yMin}");*/
                
                
                
            }

        }
        
    }
}
