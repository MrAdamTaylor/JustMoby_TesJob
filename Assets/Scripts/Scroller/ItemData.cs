using System;
using FancyScrollView;

public class ItemData
{
    public string Message { get; }
    
    public CubeData CubeData;

    public DragDropManager DragDropManager;

    public Action AdditionalAction;

    public ItemData(string message, CubeData cubeData, DragDropManager dragDropManager)
    {
        Message = message;
        CubeData = cubeData;
        DragDropManager = dragDropManager;
    }

    public void AddAction(Action stopScrolling)
    {
        AdditionalAction = stopScrolling;
    }
}