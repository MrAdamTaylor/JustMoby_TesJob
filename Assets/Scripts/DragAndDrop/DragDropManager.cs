using System;
using Common;
using Infrastructure.DI.Tickable;
using Scroller;
using UnityEngine;
using UnityEngine.EventSystems;
using ITickable = Zenject.ITickable;

namespace DragAndDrop
{
    public class DragDropManager : IDragDropManager, ITickable
    {
        private QuadItem _currentQuadItem;
        private DragDropElementView _currentDragDropElementView;
        private FancyScrollView.Scroller _scroller;
    
        private bool _isDragging;
        private OnDroppedHandler _onDroppedHandler;
        private InputHandler _inputHandler;

        public DragDropManager(FancyScrollView.Scroller scroller, DragDropElementView currentDragDropElementView, 
            OnDroppedHandler onDroppedHandler, InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            _inputHandler.OnMouseUp += HandleMouseUp;
        
            _onDroppedHandler = onDroppedHandler;
            _scroller = scroller;
            _currentDragDropElementView = currentDragDropElementView;
        }

        private void HandleMouseUp()
        {
            var endEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition,
                pointerDrag = _currentDragDropElementView?.gameObject
            };
            _currentDragDropElementView?.OnEndDrag(endEventData);
            _isDragging = false;
        }

        public void StartDrag(QuadItem quadItem, Vector2 screenPosition)
        {
            _currentDragDropElementView.Initialize(quadItem, screenPosition);
    
            _currentDragDropElementView.OnEndDragEvent += HandleEndDrag;
            _onDroppedHandler.Subscribe(_currentDragDropElementView);

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
        }
    
        private void HandleEndDrag(PointerEventData eventData)
        {
            _scroller.enabled = true;
        
            if (_currentDragDropElementView == null) 
                return;
            _currentDragDropElementView.HideHandler();
            _currentDragDropElementView.OnEndDragEvent -= HandleEndDrag;
            _onDroppedHandler.Unsubscribe(_currentDragDropElementView);
        }

        public void Dispose()
        {
            _inputHandler.OnMouseUp -= HandleMouseUp;
            GC.SuppressFinalize(this);
        }
    }
}