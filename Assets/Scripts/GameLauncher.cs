using System.Linq;
using Core;
using DragAndDrop;
using Factory;
using Infrastructure.DI.Container;
using Infrastructure.DI.Model;
using Infrastructure.DI.Tickable;
using Scroller;
using StaticData;
using UnityEngine;

public class GameLauncher : MonoBehaviour
{
    private const string PATH_TO_MAIN_UI = "Prefabs/GameContentUI";

    [SerializeField] private Canvas _canvas;
    
    private Container _container;

    void Start()
    {
        var cubeSet = LoadAllConfigs(out var mainConfig, out var towersConfig, out var mainUI);

        CreateContainer();

        var gameContentProvider = CreateUiAndGetProvider(mainUI);

        var dragDropScrollView = CreateAndBindServices(gameContentProvider, out var dragDropManager, towersConfig);
        FullDataForScrollView(cubeSet, dragDropManager, dragDropScrollView);

        var objectsCreator = new ObjectsCreator();
        _container.Construct(objectsCreator);
        objectsCreator.Init(_canvas, mainConfig, towersConfig, _container);
        
        _container.InitializeITickable();
    }

    void Update()
    {
        _container.Update();
    }

    private GameContentProvider CreateUiAndGetProvider(GameObject mainUI)
    {
        var uiInstance = Instantiate(mainUI, _canvas.transform, false);
        var gameContentProvider = uiInstance.GetComponent<GameContentProvider>();
        return gameContentProvider;
    }

    private static CubeSet LoadAllConfigs(out MainGameConfigs mainConfig, out TowersConfig towersConfig,
        out GameObject mainUI)
    {
        var cubeSet = Resources.Load<CubeSet>("Configs/NewCubeSet");
        mainConfig = Resources.Load<MainGameConfigs>("Configs/GameConfigs");
        towersConfig = Resources.Load<TowersConfig>("Configs/TowersConfig");
        mainUI = Resources.Load<GameObject>(PATH_TO_MAIN_UI);
        return cubeSet;
    }

    private void CreateContainer()
    {
        _container = new Container();
        _container.CreateScope();
    }

    private DragDropScrollView CreateAndBindServices(GameContentProvider gameContentProvider,
        out DragDropManager dragDropManager, TowersConfig towersConfig)
    {
        _container.Bind<TowersConfig>().AsScriptable(towersConfig).Registration();
        
        _container.BindData(typeof(UIFactory), typeof(UIFactory), LifeTime.Singleton);
        
        _container.Bind<TowerBuildSlotView>().AsMono(gameContentProvider.TowerBuildSlotView).Registration();
        
        var dragDropScrollView = gameContentProvider.DragDropScrollView;
        var onDroppedHandler = new OnDroppedHandler();
        onDroppedHandler.Add(gameContentProvider.TowerBuildSlotView);
        _container.Bind<OnDroppedHandler>().AsCached(onDroppedHandler).Registration();

        var inputHandler = new InputHandler();
        _container.Bind<InputHandler>().AsCached(inputHandler).AsUpdate<ITickable>().Registration();
        
        dragDropManager = new DragDropManager(gameContentProvider.Scroller, 
            gameContentProvider.DragDropElementView, onDroppedHandler, inputHandler);
        _container.Bind<IDragDropManager>().AsCached(dragDropManager).AsUpdate<ITickable>().Registration();
        return dragDropScrollView;
    }

    private static void FullDataForScrollView(CubeSet cubeSet, DragDropManager dragDropManager,
        DragDropScrollView dragDropScrollView)
    {
        var items = Enumerable.Range(0, cubeSet.CubeSets.Count)
            .Select(i => new QuadItem(cubeSet.CubeSets[i], dragDropManager))
            .ToArray();
        dragDropScrollView.UpdateData(items);
    }
}