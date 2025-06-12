using System;
using System.Collections.Generic;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "NewCubeSet", menuName = "CubeSet")]
    public class CubeSet : ScriptableObject
    {
        public List<QuadData> CubeSets;
    }

    [Serializable]
    public struct QuadData
    {
        public string ColorName;
        public Sprite CubeSprite;
    }
}