using Core;
using DragAndDrop;
using ObjectPool;
using Scroller;
using StaticData;
using UnityEngine;

namespace Factory
{
    public class UIFactory 
    {
        public GameObject CreateTowerSlot(string path,DragInformation dragInformation)
        {
            GameObject ui = Object.Instantiate(Resources.Load<GameObject>(path));

            if (ui.TryGetComponent(out TowerUpSlot towerUpSlot))
            {
                towerUpSlot.DropArea.SetParent(dragInformation.DropArea);
                towerUpSlot.DropArea.localScale = Vector3.one;
                towerUpSlot.DropArea.anchoredPosition = 
                    new Vector2(dragInformation.LocalPoint.x, dragInformation.LocalPoint.y+dragInformation.Height);
            }

            return ui;
        }

        public GameObject CreateTowerElement(string path, IDragDropManager dragDropManager, GameObject parent)
        {
            GameObject ui = Object.Instantiate(Resources.Load<GameObject>(path), parent.transform);
            if (ui.TryGetComponent(out CustomButton customButton) && ui.TryGetComponent(out TowerQuadElement towerQuad))
            {
                var data = new QuadData();
                var item = new QuadItem(data, dragDropManager);
                customButton.OnPointerDownAction += ()=>dragDropManager.StartDrag(item, Input.mousePosition);
                towerQuad.Init(item, customButton, dragDropManager);
            }
            return ui;
        }
    }
}
