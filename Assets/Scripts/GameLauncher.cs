using System;
using System.Linq;
using Infrastructure.DI.Container;
using Infrastructure.DI.Model;
using Infrastructure.DI.Tickable;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameLauncher : MonoBehaviour
{
    private const string PATH_TO_MAIN_UI = "Prefabs/GameContentUI";

    [SerializeField] private Canvas _canvas;
    
    private Container _container;


    void Start()
    {
        var cubeSet = Resources.Load<CubeSet>("Configs/NewCubeSet");
        var mainConfig = Resources.Load<MainGameConfigs>("Configs/GameConfigs");
        var mainUI = Resources.Load<GameObject>(PATH_TO_MAIN_UI);
        
        _container = new Container();
        _container.CreateScope();
        
        GameObject uiInstance = Object.Instantiate(mainUI, _canvas.transform, false);
        GameContentProvider gameContentProvider = uiInstance.GetComponent<GameContentProvider>();
        
        DragDropScrollView dragDropScrollView = gameContentProvider.DragDropScrollView;
        
        DragDropManagerNoMono dragDropManager = new DragDropManagerNoMono(gameContentProvider.Scroller, 
            gameContentProvider.DragDropElement, gameContentProvider.TownBuildSlot);
        
        _container.Bind<DragDropManagerNoMono>().AsCached(dragDropManager).AsUpdate<ITickable>().Registration();

        var items = Enumerable.Range(0, cubeSet.CubeSets.Count)
            .Select(i => new ItemData($"Cell {i}", cubeSet.CubeSets[i], dragDropManager))
            .ToArray();
        
        dragDropScrollView.UpdateData(items);

        GameObject pool = new GameObject("BuildPool");
        pool.transform.SetParent(_canvas.transform, false);

        for (int i = 0; i < mainConfig.PoolCounts; i++)
        {
            pool.transform.CreateChildImageComponent("Element"+i);
        }
        
        _container.InitializeITickable();
    }

    void Update()
    {
        _container.Update();
    }

    
    
}