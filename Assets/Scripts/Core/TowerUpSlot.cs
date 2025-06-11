using DragAndDrop;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerUpSlot : BaseSlot
{
    public RectTransform DropArea => _dropArea;
    
    private QuadTown _quadTown;

    public void SetTown(QuadTown quadTown)
    {
        _quadTown = quadTown;
    }

    public override void OnDropped(PointerEventData eventData)
    {
        base.OnDropped(eventData);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _dropArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        if (eventData.pointerDrag.TryGetComponent(out RectTransform dropedRect) &&
            eventData.pointerDrag.TryGetComponent(out DragDropElementView elementView))
        {
                
            DragInformation dragInformation = new DragInformation(localPoint, _dropArea, elementView.DragSprite, dropedRect.rect.height);
            SetToTown(dragInformation);
            Debug.Log($"<color=magenta>Right Drop at Local Position: {localPoint}</color>");
        }
    }

    private void SetToTown(DragInformation dragInformation)
    {
        _quadTown.AddToTown(dragInformation);
    }
}

public abstract class BaseSlot : MonoBehaviour, IOnDropped
{
    [SerializeField] protected RectTransform _dropArea;
    public virtual void OnDropped(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(_dropArea, eventData.position,
                eventData.pressEventCamera))
        {
            Debug.Log($"Logic in Base Class!");
            return;
        }
    }
}
