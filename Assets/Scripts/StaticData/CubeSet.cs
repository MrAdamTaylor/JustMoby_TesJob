using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewCubeSet", menuName = "CubeSet")]
public class CubeSet : ScriptableObject
{
    public List<CubeData> CubeSets;
}

[Serializable]
public struct CubeData
{
    public string ColorName;
    public Sprite CubeSprite;
}
