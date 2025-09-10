using UnityEngine;

namespace Items.Data.Product
{
    [CreateAssetMenu(fileName = "Product Item", menuName = "Data / Item / Type / Product Item")]
    public class ProductSo : ItemSo
    {
        [SerializeField] private float sparkParticleDuration;

        public float SparkParticleDuration => sparkParticleDuration;
    }
}
