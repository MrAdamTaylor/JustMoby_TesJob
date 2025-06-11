using FancyScrollView;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropManager : MonoBehaviour, IDragDropManager
{
    [SerializeField] private Scroller _scroller;
    [SerializeField] private DragDropElementView _currentDragDropElementView;
    private ItemData _currentItemData;
    private bool _isDragging;

    public void StartDrag(ItemData itemData, Vector2 screenPosition)
    {
        _currentDragDropElementView.enabled = true;
        _currentDragDropElementView.Initialize(itemData, screenPosition);
        
        _currentDragDropElementView.OnEndDragEvent += HandleEndDrag;
        
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition,
            button = PointerEventData.InputButton.Left
        };
        _currentDragDropElementView.OnBeginDrag(eventData);
        
        _isDragging = true;
    }
    
    private void Update()
    {
        if (!_isDragging) return;
        
        if (_currentDragDropElementView != null)
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
                delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
            };
            _currentDragDropElementView.OnDrag(eventData);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            var endEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
                button = PointerEventData.InputButton.Left
            };
            _currentDragDropElementView.OnEndDrag(endEventData);
            _isDragging = false;
        }
    }

    private void HandleEndDrag(PointerEventData eventData)
    {
        _scroller.enabled = true;
        
        if (_currentDragDropElementView == null) return;
        _currentDragDropElementView.enabled = false;
        _currentDragDropElementView.OnEndDragEvent -= HandleEndDrag;
    }
}
