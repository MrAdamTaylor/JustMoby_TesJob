using Core;
using DragAndDrop;
using Factory;
using Infrastructure.DI.Container;
using Infrastructure.DI.Injector;
using Infrastructure.DI.Model;
using ObjectPool;
using StaticData;
using UnityEngine;

public class ObjectsCreator
{
    private const string PATH_TO_TOWER_ELEMENT = "Prefabs/TowerQuad";
    
    private UIFactory _factory;
    private IDragDropManager _dragDropManager;

    [Inject]
    public void Construct(UIFactory factory, IDragDropManager dragDropManager)
    {
        _dragDropManager = dragDropManager;
        _factory = factory;
    }
    
    public void Init(Canvas canvas, MainGameConfigs mainConfig, TowersConfig towersConfig, Container container)
    {
        GameObject pool = new GameObject("BuildPool");
        pool.transform.SetParent(canvas.transform, false);

        var objectPool = new QuadObjectPool(mainConfig.PoolCounts * towersConfig.MaxTowers, 
            ()=> _factory.CreateTowerElement(PATH_TO_TOWER_ELEMENT, _dragDropManager, pool), pool);
        container.Bind<QuadObjectPool>().AsCached(objectPool).Registration();
        
        QuadTowerCreator quadTowerCreator = new QuadTowerCreator();
        container.Construct(quadTowerCreator);
        quadTowerCreator.Configure();
    }
}