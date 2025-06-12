using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "GameConfigs", menuName = "MainGameConfig")]
    public class MainGameConfigs : ScriptableObject
    {
        [Range(5, 10)]
        public int PoolCounts = 5;
    }
}
