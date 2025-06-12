using System;
using DragAndDrop;
using StaticData;

namespace Scroller
{
    public class ItemData
    {
    
        public CubeData CubeData;

        public IDragDropManager DragDropManager;

        public Action AdditionalAction;

        public ItemData(CubeData cubeData, IDragDropManager dragDropManager)
        {
            CubeData = cubeData;
            DragDropManager = dragDropManager;
        }

        public void AddAction(Action stopScrolling)
        {
            AdditionalAction = stopScrolling;
        }
    }
}