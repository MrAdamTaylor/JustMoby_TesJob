using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpSlot : MonoBehaviour
{
    [SerializeField] private RectTransform _dropArea;
    
    public RectTransform DropArea => _dropArea;
}
