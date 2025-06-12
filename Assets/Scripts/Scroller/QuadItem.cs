using System;
using DragAndDrop;
using StaticData;

namespace Scroller
{
    public class QuadItem
    {
        public QuadData QuadData;

        public readonly IDragDropManager DragDropManager;

        public Action AdditionalAction;

        public QuadItem(QuadData quadData, IDragDropManager dragDropManager)
        {
            QuadData = quadData;
            DragDropManager = dragDropManager;
        }

        public void AddAction(Action stopScrolling)
        {
            AdditionalAction = stopScrolling;
        }
    }
}