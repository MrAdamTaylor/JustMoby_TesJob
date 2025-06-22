using System.Diagnostics;
using System.Linq;
using Core;
using Core.Tower;
using DragAndDrop;
using Factory;
using Infrastructure.DI.Container;
using Infrastructure.DI.Model;
using Infrastructure.DI.Tickable;
using Localization;
using Scroller;
using StaticData;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Common
{
    public class GameInstaller : MonoBehaviour
    {
        private const string PATH_TO_MAIN_UI = "Prefabs/GameContentUI";

        [SerializeField] private Canvas _canvas;
    
        private Container _container;

        void Start()
        {
            var cubeSet = LoadAllConfigs(out var mainConfig, out var towersConfig, out var mainUI);

            var localizationManager = new LocalizationManager();
            localizationManager.LoadLocalization();
        
            CreateContainer();

            var gameContentProvider = CreateUiAndGetProvider(mainUI);

            localizationManager.AddILocalizable(gameContentProvider.Message);
        
            //Stopwatch timeAllContainer = new Stopwatch();
            //timeAllContainer.Start();
            var dragDropScrollView = CreateAndBindServices(gameContentProvider, out var dragDropManager, towersConfig);
            FullDataForScrollView(cubeSet, dragDropManager, dragDropScrollView);
            var trashHolder = new TrashHolder(gameContentProvider.TrashSlot, gameContentProvider.MovingElement, gameContentProvider.Message);
            var objectsCreator = new ObjectsCreator();
            
            //Stopwatch injectTime = new Stopwatch();
            //injectTime.Start();
            _container.Construct(objectsCreator);
            //timeAllContainer.Stop();
            //Debug.Log($"Время регистрации и инджекта всех зависимостей {timeAllContainer.ElapsedMilliseconds}");
            //injectTime.Stop();
            //Debug.Log($"Время инджекта одной зависимости {injectTime.ElapsedMilliseconds}");
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
            Stopwatch createDI = new Stopwatch();
            createDI.Start();
            _container = new Container();
            _container.CreateScope();
            createDI.Stop();
            Debug.Log($"Время создания моего DI {createDI.ElapsedMilliseconds} ms");
        }

        private DragDropScrollView CreateAndBindServices(GameContentProvider gameContentProvider,
            out DragDropManager dragDropManager, TowersConfig towersConfig)
        {
            //Stopwatch bindTime = new Stopwatch();
            //bindTime.Start();
            _container.Bind<TowersConfig>().AsScriptable(towersConfig).Registration();
            //bindTime.Stop();
            //Debug.Log($"Время регистрации одной зависимости {bindTime.ElapsedMilliseconds}");
            
            Stopwatch createTime = new Stopwatch();
            createTime.Start();
            _container.BindData(typeof(UIFactory), typeof(UIFactory), LifeTime.Singleton);
            createTime.Stop();
            Debug.Log($"Время регистрации одной зависимости {createTime.ElapsedMilliseconds} ms");
        
            _container.Bind<TowerBuildSlotView>().AsMono(gameContentProvider.TowerBuildSlotView).Registration();
            _container.Bind<MessageOutput>().AsMono(gameContentProvider.Message).Registration();
        
            var dragDropScrollView = gameContentProvider.DragDropScrollView;
            var onDroppedHandler = new OnDroppedHandler();
            onDroppedHandler.Add(gameContentProvider.TowerBuildSlotView);
            onDroppedHandler.Add(gameContentProvider.TrashSlot);

            Stopwatch createManager = new Stopwatch();
            createManager.Start();
            var inputHandler = new InputHandler();
            dragDropManager = new DragDropManager(gameContentProvider.Scroller, 
                gameContentProvider.DragDropElementView, onDroppedHandler, inputHandler);
            
            _container.Bind<OnDroppedHandler>().AsCached(onDroppedHandler).Registration();
            _container.Bind<InputHandler>().AsCached(inputHandler).AsUpdate<ITickable>().Registration();
            _container.Bind<IDragDropManager>().AsCached(dragDropManager).AsUpdate<ITickable>().Registration();
            createManager.Stop();
            Debug.Log($"Время создания сервиса и регистраций {createTime.ElapsedMilliseconds} ms");
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
}