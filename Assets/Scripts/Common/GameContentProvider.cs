using Core;
using Core.Slot;
using Core.Tower;
using DragAndDrop;
using Scroller;
using UnityEngine;

namespace Common
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
