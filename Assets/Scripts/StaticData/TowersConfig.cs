using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "TowersConfigs", menuName = "TowersConfig")]
    public class TowersConfig : ScriptableObject
    {
        [Range(1,3)] 
        [SerializeField] private int _maxTowers = 1;

        public int MaxTowers => _maxTowers;
        
        public bool CheckByCount(int count) => count < _maxTowers;
    }
}
