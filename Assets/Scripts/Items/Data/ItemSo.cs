using System.Collections.Generic;
using System.Linq;
using Items.Type;
using UnityEngine;

namespace Items.Data
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Data / Item / Type / Base Item")]
    public class ItemSo : ScriptableObject
    {
        [SerializeField] private BaseItem prefab;
        [SerializeField] private List<LevelSo> levels;
        
        public BaseItem Prefab => prefab;
        public int MaxLevel => levels.Count;

        public LevelSo GetNextLevel(LevelSo currentLevelSo)
        {
            var targetLevel = currentLevelSo.Level + 1;
            return levels.FirstOrDefault(levelSo => levelSo.Level == targetLevel);
        }
    }
}
