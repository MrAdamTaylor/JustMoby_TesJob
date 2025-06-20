using System.Collections.Generic;
using Core.Slot;
using DG.Tweening;
using DragAndDrop;
using Localization;
using ObjectPool;
using UniRx;
using UnityEngine;

namespace Core.Tower
{
    public class QuadTower
    {
        private const float ANIMATION_TIME = 0.5f;

        public ReactiveCommand<QuadTower> OnQuadTowerZero = new();
        
        private LinkedList<GameObject> _town = new();
        private GameObject _slot;
        private RectTransform _slotRectTransform;
        private RectTransform _mainArea;
        private QuadObjectPool _quadObjectPool;
        private MessageOutput _messageOutput;

        public QuadTower(GameObject slot, GameObject gameObject, QuadObjectPool quadObjectPool, RectTransform mainArea, MessageOutput messageOutput)
        {
            _messageOutput = messageOutput;
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
            if (_mainArea.rect.yMax - dragInformation.Height * 0.5f < _slotRectTransform.anchoredPosition.y)
            {
                _messageOutput.OutputByKey(MessagesKey.TO_HEIGHT);
                return;
            }
            _messageOutput.OutputByKey(MessagesKey.ADD_TO_LADER);
            dragInformation.FlagSetter.SetFlag(true);
            Vector2 positionLastTown = _town.Last.Value.GetComponent<RectTransform>().anchoredPosition;
            DragInformation dragInformation2 = dragInformation;
            Vector2 position = new Vector2(positionLastTown.x + (dragInformation.LocalPoint.x/2) , positionLastTown.y + dragInformation.Height);
        
            dragInformation2.LocalPoint = position;
            dragInformation2.DropArea = _mainArea;
            GameObject element = _quadObjectPool.SpawnAtPosition(dragInformation2);
            if (element.TryGetComponent(out TowerQuadElement towerQuad))
            {
                towerQuad.Set(dragInformation.Sprite, dragInformation.ColorName, MoveTower);
            }
            _town.AddLast(element);
            _slotRectTransform.anchoredPosition = new Vector2(position.x, position.y+dragInformation.Height);
        }

        public void MoveTower(GameObject element)
        {
            _messageOutput.OutputByKey(MessagesKey.FROM_LADER);
            var queue = new Queue<GameObject>();
            var currentNode = _town.Find(element);

            Vector2 previousPoint = new Vector2();

            if (currentNode != null)
            {
                if (currentNode.Value.TryGetComponent(out RectTransform rectTransform)) 
                {
                    previousPoint = rectTransform.anchoredPosition;
                }
                
                var nextNode = currentNode.Next;

                while (nextNode != null)
                {
                    if (nextNode.Value.TryGetComponent(out TowerQuadElement towerQuadElement))
                    {
                        queue.Enqueue(nextNode.Value);
                    }
                    nextNode = nextNode.Next;
                }
                _town.Remove(currentNode);
                _quadObjectPool.Return(currentNode.Value);
            }
            else
            {
                Debug.LogWarning("Element not found in the town list.");
            }
            
            Sequence moveSequence = DOTween.Sequence();
            
            while (queue.Count > 0)
            {
                GameObject nextElement = queue.Dequeue();
                if (nextElement.TryGetComponent(out RectTransform rectTransform))
                {
                    rectTransform.DOKill();
                    moveSequence.Insert(0, rectTransform.DOAnchorPos(previousPoint, ANIMATION_TIME));
                    previousPoint = rectTransform.anchoredPosition;
                }
            }

            if (_town.Count == 0)
            {
                OnQuadTowerZero?.Execute(this);
            }
        }
    }
}