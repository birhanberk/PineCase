using System.Collections.Generic;
using UnityEngine;

namespace Orders
{
    [CreateAssetMenu(fileName = "Order Data", menuName = "Data / Order / Order List")]
    public class OrderSo : ScriptableObject
    {
        [SerializeField] private int displayCount;
        [SerializeField] private List<OrderData> orders = new ();

        public int DisplayCount => displayCount;
        public List<OrderData> Orders => orders;
    }
}
