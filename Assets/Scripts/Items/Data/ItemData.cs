using System;
using UnityEngine;

namespace Items.Data
{
    [Serializable]
    public class ItemData
    {
        [SerializeField] private ItemSo itemSo;
        [SerializeField] private LevelSo levelSo;

        public ItemSo ItemSo => itemSo;
        public LevelSo LevelSo => levelSo;

        public ItemData(ItemData data)
        {
            itemSo = data.ItemSo;
            levelSo = data.LevelSo;
        }

        public ItemData(ItemSo itemSo, LevelSo levelSo)
        {
            this.itemSo = itemSo;
            this.levelSo = levelSo;
        }
        
        public void UpgradeLevel()
        {
            levelSo = itemSo.GetNextLevel(levelSo);
        }
    }
}
