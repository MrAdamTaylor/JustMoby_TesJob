using Scroller;
using UnityEngine;

namespace DragAndDrop
{
    public interface IDragDropManager
    {
        public void StartDrag(QuadItem quadItem, Vector2 screenPosition);
    }
}