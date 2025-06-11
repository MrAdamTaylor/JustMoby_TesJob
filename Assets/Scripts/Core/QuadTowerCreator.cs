using System.Collections.Generic;
using DragAndDrop;
using Infrastructure.DI.Injector;
using UnityEngine;

public class QuadTowerCreator
{
    private const string PATH_TO_SLOT = "Prefabs/TowerSlot";
    
    [Inject] private TowerBuildSlotView _view;
    [Inject] private TowersConfig _towersConfig;
    [Inject] private QuadObjectPool _quadObjectPool; 
    [Inject] private UIFactory _uiFactory;
    [Inject] private OnDroppedHandler _onDropped;
    
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
            var element = _quadObjectPool.SpawnAtPosition(dragInformation);
            
            GameObject slot = _uiFactory.CreateTowerSlot(PATH_TO_SLOT, dragInformation);

            if (slot.TryGetComponent(out TowerUpSlot towerUpSlot))
            {
                _onDropped.Add(towerUpSlot);
            }
            
            _quadTowns.Add(new QuadTown(slot,element, _quadObjectPool));
        }
    }
}

public class QuadTown
{
    private LinkedList<GameObject> _town = new();
    private GameObject _slot;
    private RectTransform _slotRectTransform;
    private GameObject _gameObject;
    private QuadObjectPool _quadObjectPool;

    public QuadTown(GameObject slot, GameObject gameObject, QuadObjectPool quadObjectPool)
    {
        _gameObject = gameObject;
        _slot = slot;
        _slotRectTransform = slot.GetComponent<RectTransform>();
        _slot.TryGetComponent(out TowerUpSlot towerUpSlot);
        _quadObjectPool = quadObjectPool;
        towerUpSlot.SetTown(this);
    }

    public void AddToTown(DragInformation dragInformation)
    {
        DragInformation dragInformation2 = dragInformation;
        
        dragInformation2.LocalPoint = _slotRectTransform.anchoredPosition;
        GameObject element = _quadObjectPool.SpawnAtPosition(dragInformation);
        _town.AddLast(element);
        _slotRectTransform.anchoredPosition = new Vector2(_slotRectTransform.anchoredPosition.x, _slotRectTransform.anchoredPosition.y+dragInformation.Height);
        
    }
}
