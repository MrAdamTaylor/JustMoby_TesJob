using System;
using DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

public class QuadObjectPool 
{
    private GameObjectPool _pool;

    public void Construct(int totalCount, Func<GameObject> factory)
    {
        _pool = new GameObjectPool(factory, totalCount);
    }
    
    
    public GameObject SpawnAtPosition(DragInformation information)
    {
        GameObject quad = _pool.Get();
        if(quad.TryGetComponent(out Image image) && quad.TryGetComponent(out RectTransform rectTransform))
        {
            rectTransform.SetParent(information.DropArea);
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition = information.LocalPoint;
            image.sprite = information.Sprite;
        }
        return quad;
    }
    
    public void Return(GameObject quad)
    {
        quad.SetActive(false);
        _pool.Return(quad);
    }
}
