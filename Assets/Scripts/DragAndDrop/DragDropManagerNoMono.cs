using System;
using DragAndDrop;
using FancyScrollView;
using Infrastructure.DI.Tickable;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropManagerNoMono : IDragDropManager, ITickable
{
    private ItemData _currentItemData;
    private DragDropElement _currentDragDropElement;
    private Scroller _scroller;
    
    private bool _isDragging;
    private TownBuildSlot _townBuildSlot;

    public DragDropManagerNoMono(Scroller scroller, DragDropElement currentDragDropElement, TownBuildSlot townBuildSlot)
    {
        _townBuildSlot = townBuildSlot;
        _scroller = scroller;
        _currentDragDropElement = currentDragDropElement;
    }
    
    public void StartDrag(ItemData itemData, Vector2 screenPosition)
    {
        _currentDragDropElement.enabled = true;
        _currentDragDropElement.Initialize(itemData, screenPosition);
    
        _currentDragDropElement.OnEndDragEvent += HandleEndDrag;
        _currentDragDropElement.OnEndDragEvent += _townBuildSlot.OnDrop;

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition,
            pointerDrag = _currentDragDropElement.gameObject
        };
        _currentDragDropElement.OnBeginDrag(eventData);
    
        _isDragging = true;
    }
    
    public void Tick()
    {
        if (!_isDragging) return;
        
        if (_currentDragDropElement != null)
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
                delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
            };
            _currentDragDropElement.OnDrag(eventData);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            var endEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
                pointerDrag = _currentDragDropElement.gameObject
            };
            _currentDragDropElement.OnEndDrag(endEventData);
            _isDragging = false;
        }
    }
    
    private void HandleEndDrag(PointerEventData eventData)
    {
        _scroller.enabled = true;
        
        if (_currentDragDropElement == null) return;
        _currentDragDropElement.enabled = false;
        _currentDragDropElement.OnEndDragEvent -= HandleEndDrag;
        _currentDragDropElement.OnEndDragEvent -= _townBuildSlot.OnDrop;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
