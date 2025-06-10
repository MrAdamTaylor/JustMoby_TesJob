using System.Linq;
using Infrastructure.DI.Container;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    private const string PATH_TO_MAIN_UI = "Prefabs/GameContentUI";

    [SerializeField] private Canvas _canvas;
    
    private Container _container;


    void Start()
    {
        var cubeSet = Resources.Load<CubeSet>("Configs/NewCubeSet");
        var mainUI = Resources.Load<GameObject>(PATH_TO_MAIN_UI);
        
        _container = new Container();
        _container.CreateScope();
        
        GameObject uiInstance = Object.Instantiate(mainUI, _canvas.transform, false);
        GameContentProvider gameContentProvider = uiInstance.GetComponent<GameContentProvider>();

        DragDropManager dragDropManager = gameContentProvider.DragDropManager;
        DragDropScrollView dragDropScrollView = gameContentProvider.DragDropScrollView;

        var items = Enumerable.Range(0, cubeSet.CubeSets.Count)
            .Select(i => new ItemData($"Cell {i}", cubeSet.CubeSets[i], dragDropManager))
            .ToArray();
        
        dragDropScrollView.UpdateData(items);
    }
    
}
