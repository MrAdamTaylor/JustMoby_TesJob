using System;
using DragAndDrop;
using FancyScrollView;
using Infrastructure.DI.Tickable;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropManagerNoMono : IDragDropManager, ITickable
{
    private ItemData _currentItemData;
    private DragDropElementView _currentDragDropElementView;
    private Scroller _scroller;
    
    private bool _isDragging;
    private TowerBuildSlotView _towerBuildSlotView;

    public DragDropManagerNoMono(Scroller scroller, DragDropElementView currentDragDropElementView, TowerBuildSlotView towerBuildSlotView)
    {
        _towerBuildSlotView = towerBuildSlotView;
        _scroller = scroller;
        _currentDragDropElementView = currentDragDropElementView;
    }
    
    public void StartDrag(ItemData itemData, Vector2 screenPosition)
    {
        _currentDragDropElementView.enabled = true;
        _currentDragDropElementView.Initialize(itemData, screenPosition);
    
        _currentDragDropElementView.OnEndDragEvent += HandleEndDrag;
        _currentDragDropElementView.OnEndDragEvent += _towerBuildSlotView.OnDrop;

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition,
            pointerDrag = _currentDragDropElementView.gameObject
        };
        _currentDragDropElementView.OnBeginDrag(eventData);
    
        _isDragging = true;
    }
    
    public void Tick()
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
                pointerDrag = _currentDragDropElementView.gameObject
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
        _currentDragDropElementView.OnEndDragEvent -= _towerBuildSlotView.OnDrop;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
