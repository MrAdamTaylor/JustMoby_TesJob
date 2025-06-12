using System.Collections.Generic;
using DragAndDrop;
using UnityEngine;

public class QuadTown
{
    private LinkedList<GameObject> _town = new();
    private GameObject _slot;
    private RectTransform _slotRectTransform;
    private RectTransform _mainArea;
    //private GameObject _gameObject;
    private QuadObjectPool _quadObjectPool;

    public QuadTown(GameObject slot, GameObject gameObject, QuadObjectPool quadObjectPool, RectTransform mainArea)
    {
        //_gameObject = gameObject;
        _mainArea = mainArea;
        _town.AddLast(gameObject);
        _slot = slot;
        _slotRectTransform = slot.GetComponent<RectTransform>();
        _slot.TryGetComponent(out TowerUpSlot towerUpSlot);
        _quadObjectPool = quadObjectPool;
        towerUpSlot.SetTown(this);
    }

    public void AddToTown(DragInformation dragInformation)
    {
        if(_mainArea.rect.yMax - dragInformation.Height < _slotRectTransform.anchoredPosition.y)
            return;
        
        Vector2 positionLastTown = _town.Last.Value.GetComponent<RectTransform>().anchoredPosition;
        DragInformation dragInformation2 = dragInformation;
        Vector2 position = new Vector2(dragInformation.LocalPoint.x/2, positionLastTown.y + dragInformation.Height);
        
        dragInformation2.LocalPoint = position;
        dragInformation2.DropArea = _mainArea;
        GameObject element = _quadObjectPool.SpawnAtPosition(dragInformation2);
        _town.AddLast(element);
        _slotRectTransform.anchoredPosition = new Vector2(position.x, position.y+dragInformation.Height);
        
    }
}