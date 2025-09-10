using UnityEngine;

namespace Items.Data.Common
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data / Item / Level / Resource Level")]
    public class ResourceLevelSo : LevelSo
    {
        [SerializeField] private int rewardCount;

        public int RewardCount => rewardCount;
    }
}
