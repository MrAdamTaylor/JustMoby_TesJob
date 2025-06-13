using System.Collections.Generic;
using Core.Slot;
using DragAndDrop;
using Factory;
using Infrastructure.DI.Injector;
using Localization;
using ObjectPool;
using StaticData;
using UniRx;
using UnityEngine;

namespace Core.Tower
{
    public class QuadTowerCreator
    {
        private const string PATH_TO_SLOT = "Prefabs/TowerSlot";
    
        private TowerBuildSlotView _view;
        private TowersConfig _towersConfig;
        private QuadObjectPool _quadObjectPool; 
        private UIFactory _uiFactory;
        private OnDroppedHandler _onDropped;
        private MessageOutput _messageOutput;
    
        private readonly List<QuadTower> _quadTowers = new();
        private readonly CompositeDisposable _compositeDisposable = new();

        [Inject]
        public void Construct(TowerBuildSlotView view, QuadObjectPool quadObjectPool, TowersConfig config, 
            UIFactory uiFactory, OnDroppedHandler onDropped, MessageOutput messageOutput)
        {
            _view = view; 
            _quadObjectPool = quadObjectPool;
            _uiFactory = uiFactory;
            _onDropped = onDropped;
            _messageOutput = messageOutput;
            _towersConfig = config;
        }

        public void Configure()
        {
            _view.OnDragStart += CreateQuadTown;
        }

        ~QuadTowerCreator()
        {
            _view.OnDragStart -= CreateQuadTown;
            _compositeDisposable.Clear();
        }

        private void CreateQuadTown(DragInformation dragInformation)
        {
            if (_towersConfig.CheckByCount(_quadTowers.Count))
            {
                dragInformation.FlagSetter.SetFlag(true);
                var element = _quadObjectPool.SpawnAtPosition(dragInformation);

                GameObject slot = _uiFactory.CreateTowerSlot(PATH_TO_SLOT, dragInformation);

                if (slot.TryGetComponent(out TowerUpSlot towerUpSlot))
                {
                    _onDropped.Add(towerUpSlot);
                }

                var quadTower = new QuadTower(slot, element, _quadObjectPool, dragInformation.DropArea, _messageOutput);
                quadTower.OnQuadTowerZero.First().Subscribe(p => RemoveTower(p)).AddTo(_compositeDisposable);
                if (element.TryGetComponent(out TowerQuadElement towerQuad))
                {
                    towerQuad.Set(dragInformation.Sprite, dragInformation.ColorName, quadTower.MoveTower);
                }
                _quadTowers.Add(quadTower);
                _messageOutput.OutputByKey(MessagesKey.ADD_TO_LADER);
            }
        }

        private void RemoveTower(QuadTower quadTower)
        {
            _quadTowers.Remove(quadTower);
        }
        
    }
}