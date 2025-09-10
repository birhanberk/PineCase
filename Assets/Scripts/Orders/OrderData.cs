using System;
using System.Collections.Generic;
using Items.Data;
using UnityEngine;

namespace Orders
{
    [Serializable]
    public class OrderData
    {
        [SerializeField] private int rewardAmount;
        [SerializeField] private List<ItemData> items = new ();

        public int RewardAmount => rewardAmount;
        public List<ItemData> Items => items;
    }
}
