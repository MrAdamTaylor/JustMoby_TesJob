public class ItemData
{
    public string Message { get; }
    
    public CubeData CubeData;

    public ItemData(string message, CubeData cubeData)
    {
        Message = message;
        CubeData = cubeData;
    }
}