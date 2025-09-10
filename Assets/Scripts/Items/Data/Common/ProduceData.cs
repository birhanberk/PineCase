using System;
using UnityEngine;

namespace Items.Data.Common
{
    [Serializable]
    public class ProduceData
    {
        [SerializeField] private ItemData itemData;
        [SerializeField] private int capacity;
        
        public ItemData Data => itemData;
        public int Capacity => capacity;
    }
}
