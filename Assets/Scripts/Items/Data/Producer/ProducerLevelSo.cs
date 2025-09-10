using System.Collections.Generic;
using Items.Data.Common;
using UnityEngine;

namespace Items.Data.Producer
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data / Item / Level / Producer Level")]
    public class ProducerLevelSo : LevelSo
    {
        [SerializeField] private int rechargeTime;
        [SerializeField] private List<ProduceData> produceData;

        public int RechargeTime => rechargeTime;
        public List<ProduceData> ProduceData => produceData;
    }
}
