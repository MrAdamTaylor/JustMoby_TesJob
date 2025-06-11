using DragAndDrop;
using UnityEngine;

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
}
