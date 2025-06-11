using System.Collections.Generic;
using DragAndDrop;
using Infrastructure.DI.Injector;
using UnityEngine;

public class QuadTowerCreator
{
    [Inject] private TowerBuildSlotView _view;
    [Inject] private TowersConfig _towersConfig;
    [Inject] private QuadObjectPool _quadObjectPool; 
    [Inject] private TowerUpSlot _towerUpSlot;
    
    private List<QuadTown> _quadTowns = new();

    public void Configure()
    {
        _view.OnDragStart += CreateQuadTown;
    }

    ~QuadTowerCreator()
    {
        _view.OnDragStart -= CreateQuadTown;
    }

    private void CreateQuadTown(DragInformation dragInformation)
    {
        if (_towersConfig.CheckByCount(_quadTowns.Count))
        {
            Debug.Log($"Can Create!: ");
            var gameObject = _quadObjectPool.SpawnAtPosition(dragInformation);
            
            
            _towerUpSlot.DropArea.SetParent(dragInformation.DropArea);
            _towerUpSlot.DropArea.localScale = Vector3.one;
            _towerUpSlot.DropArea.anchoredPosition = 
                new Vector2(dragInformation.LocalPoint.x, dragInformation.LocalPoint.y+dragInformation.Height);
            _quadTowns.Add(new QuadTown(gameObject));
            
            /*rectTransform.SetParent(information.DropArea);
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = information.LocalPoint;*/
        }
    }
}

public class QuadTown
{
    private LinkedList<GameObject> _town = new();

    public QuadTown(GameObject gameObject)
    {
        
    }
}
