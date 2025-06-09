using System.Linq;
using UnityEngine;

public class ScrollerInitializer : MonoBehaviour
{
    [SerializeField] private DragDropManager _dragDropManager;
    [SerializeField] private DragDropScrollView _dragDropScrollView;
    
    void Start()
    {
        var cubeSet = Resources.Load<CubeSet>("Configs/NewCubeSet");
        
        var items = Enumerable.Range(0, cubeSet.CubeSets.Count)
            .Select(i => new ItemData($"Cell {i}", cubeSet.CubeSets[i], _dragDropManager))
            .ToArray();
        
        _dragDropScrollView.UpdateData(items);
    }  
}