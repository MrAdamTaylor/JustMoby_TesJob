using System;

public class ItemData
{
    public string Message { get; }
    
    public CubeData CubeData;

    public IDragDropManager DragDropManager;

    public Action AdditionalAction;

    public ItemData(string message, CubeData cubeData, IDragDropManager dragDropManager)
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