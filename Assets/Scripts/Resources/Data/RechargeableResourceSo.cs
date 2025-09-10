using UnityEngine;

namespace Resources.Data
{
    [CreateAssetMenu(fileName = "Resource Data", menuName = "Data / Resource / Rechargeable Resource")]
    public class RechargeableResourceSo : ResourceSo
    {
        [SerializeField] private int rechargeMaxAmount;
        [SerializeField] private int rechargeInterval;

        public int RechargeMaxAmount => rechargeMaxAmount;
        public int RechargeInterval => rechargeInterval;
    }
}