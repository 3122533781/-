using UnityEngine;
using UnityEngine.Serialization;

namespace _02.Scripts.LevelEdit
{
    public class LevelConfig : ScriptableConfigGroup<LevelData, LevelConfig>
    {
        [SerializeField] private SpecialLevelConfig specialLevelConfig;

        public SpecialLevelConfig GetSpecialLevelConfig()
        {
            return specialLevelConfig;
        }
    }
}