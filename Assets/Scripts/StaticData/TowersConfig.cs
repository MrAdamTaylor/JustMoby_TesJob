using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TowersConfigs", menuName = "TowersConfig")]
    public class TowersConfig : ScriptableObject
    {
        [Range(1,3)] 
        [SerializeField] private int MaxTowers = 1;

        public bool CheckByCount(int count) => count < MaxTowers;
    }
}
