using System.Linq;
using DragAndDrop;
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
        var towersConfig = Resources.Load<TowersConfig>("Configs/TowersConfig");
        var mainUI = Resources.Load<GameObject>(PATH_TO_MAIN_UI);
        
        _container = new Container();
        _container.CreateScope();
        
        _container.Bind<TowersConfig>().AsScriptable(towersConfig).Registration();
        
        GameObject uiInstance = Instantiate(mainUI, _canvas.transform, false);
        GameContentProvider gameContentProvider = uiInstance.GetComponent<GameContentProvider>();
        
        _container.BindData(typeof(UIFactory), typeof(UIFactory), LifeTime.Transient);
        
        _container.Bind<TowerBuildSlotView>().AsMono(gameContentProvider.TowerBuildSlotView).Registration();
        
        DragDropScrollView dragDropScrollView = gameContentProvider.DragDropScrollView;
        OnDroppedHandler onDroppedHandler = new OnDroppedHandler();
        onDroppedHandler.Add(gameContentProvider.TowerBuildSlotView);
        _container.Bind<OnDroppedHandler>().AsCached(onDroppedHandler).Registration();

        InputHandler inputHandler = new InputHandler();
        _container.Bind<InputHandler>().AsCached(inputHandler).AsUpdate<ITickable>().Registration();
        
        DragDropManager dragDropManager = new DragDropManager(gameContentProvider.Scroller, 
            gameContentProvider.DragDropElementView, onDroppedHandler, inputHandler);
        
        _container.Bind<DragDropManager>().AsCached(dragDropManager).AsUpdate<ITickable>().Registration();
        
        
        var items = Enumerable.Range(0, cubeSet.CubeSets.Count)
            .Select(i => new ItemData($"Cell {i}", cubeSet.CubeSets[i], dragDropManager))
            .ToArray();
        
        dragDropScrollView.UpdateData(items);

        GameObject pool = new GameObject("BuildPool");
        pool.transform.SetParent(_canvas.transform, false);

        QuadObjectPool objectPool = new QuadObjectPool();
        objectPool.Construct(mainConfig.PoolCounts, ()=>pool.transform.CreateChildImageComponent("Element"));
        _container.Bind<QuadObjectPool>().AsCached(objectPool).Registration();
        
        
        QuadTowerCreator quadTowerCreator = new QuadTowerCreator();
        _container.Construct(quadTowerCreator);
        quadTowerCreator.Configure();
        
        _container.InitializeITickable();
    }

    void Update()
    {
        _container.Update();
    }

    
    
}