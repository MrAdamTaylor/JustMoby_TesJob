using Scroller;
using UnityEngine;

namespace DragAndDrop
{
    public interface IDragDropManager
    {
        public void StartDrag(ItemData itemData, Vector2 screenPosition);
    }
}