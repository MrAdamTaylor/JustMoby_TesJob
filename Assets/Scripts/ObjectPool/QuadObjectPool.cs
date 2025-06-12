using System;
using DragAndDrop;
using UnityEngine;
using UnityEngine.UI;

namespace ObjectPool
{
    public class QuadObjectPool 
    {
        private GameObjectPool _pool;
        private GameObject _parentObject;

        public QuadObjectPool(int totalCount, Func<GameObject> factory, GameObject parentObject)
        {
            _parentObject = parentObject;
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
            quad.transform.SetParent(_parentObject.transform);
            quad.SetActive(false);
            _pool.Return(quad);
        }
    }
}
