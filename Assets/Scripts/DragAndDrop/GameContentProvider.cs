using Core;
using Scroller;
using UnityEngine;

namespace DragAndDrop
{
    public class GameContentProvider : MonoBehaviour
    {
        public FancyScrollView.Scroller Scroller;
        public DragDropScrollView DragDropScrollView;
        public DragDropElementView DragDropElementView;
        public TowerBuildSlotView TowerBuildSlotView;
        public TrashSlot TrashSlot;
        public MovingElement MovingElement;
        public MessageOutput Message;
    }
}
