using System.Collections.Generic;
using Items.Data.Common;
using UnityEngine;

namespace Items.Data.Chest
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data / Item / Level / Chest Level")]
    public class ChestLevelSo : LevelSo
    {
        [SerializeField] private List<ProduceData> produceData;

        public List<ProduceData> ProduceData => produceData;
    }
}
