using System.Collections.Generic;
using DragAndDrop;
using Factory;
using Infrastructure.DI.Injector;
using ObjectPool;
using StaticData;
using UnityEngine;

namespace Core
{
    public class QuadTowerCreator
    {
        private const string PATH_TO_SLOT = "Prefabs/TowerSlot";
    
        [Inject] private TowerBuildSlotView _view;
        [Inject] private TowersConfig _towersConfig;
        [Inject] private QuadObjectPool _quadObjectPool; 
        [Inject] private UIFactory _uiFactory;
        [Inject] private OnDroppedHandler _onDropped;
    
        private readonly List<QuadTower> _quadTowers = new();

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
            if (_towersConfig.CheckByCount(_quadTowers.Count))
            {
                Debug.Log($"Can Create!: ");
                dragInformation.FlagSetter.SetFlag(true);
                var element = _quadObjectPool.SpawnAtPosition(dragInformation);
                

                GameObject slot = _uiFactory.CreateTowerSlot(PATH_TO_SLOT, dragInformation);

                if (slot.TryGetComponent(out TowerUpSlot towerUpSlot))
                {
                    _onDropped.Add(towerUpSlot);
                }

                var quadTower = new QuadTower(slot, element, _quadObjectPool, dragInformation.DropArea);
                quadTower.OnQuadTowerZero += RemoveTower;
                if (element.TryGetComponent(out TowerQuadElement towerQuad))
                {
                    towerQuad.Set(dragInformation.Sprite, dragInformation.ColorName, quadTower.MoveTower);
                }
                _quadTowers.Add(quadTower);
            }
        }

        private void RemoveTower(QuadTower quadTower)
        {
            quadTower.OnQuadTowerZero -= RemoveTower;
            _quadTowers.Remove(quadTower);
        }
    }
}