using FancyScrollView;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropManager : MonoBehaviour
{
    [SerializeField] private Scroller _scroller;
    [SerializeField] private DragDrop _currentDragDrop;
    private ItemData _currentItemData;
    private bool _isDragging;

    public void StartDrag(ItemData itemData, Vector2 screenPosition)
    {
        _currentDragDrop.enabled = true;
        _currentDragDrop.Initialize(itemData, screenPosition);
        
        _currentDragDrop.OnEndDragEvent += HandleEndDrag;
        
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition,
            button = PointerEventData.InputButton.Left
        };
        _currentDragDrop.OnBeginDrag(eventData);
        
        _isDragging = true;
        _currentDragDrop.OnEndDragEvent += HandleEndDrag;
    }
    
    private void Update()
    {
        if (!_isDragging) return;
        
        if (_currentDragDrop != null)
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
                delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
            };
            _currentDragDrop.OnDrag(eventData);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            var endEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
                button = PointerEventData.InputButton.Left
            };
            _currentDragDrop.OnEndDrag(endEventData);
            _isDragging = false;
        }
    }

    private void HandleEndDrag(PointerEventData eventData)
    {
        _scroller.enabled = true;
        
        if (_currentDragDrop == null) return;
        _currentDragDrop.enabled = false;
        _currentDragDrop.OnEndDragEvent -= HandleEndDrag;
    }
}
