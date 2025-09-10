using Grid;
using Items.Data;
using Items.Data.Product;
using UnityEngine;

namespace Items.Type.Product
{
    public class ProductItem : BaseItem
    {
        [SerializeField] private ItemParticlePlayer sparkleParticle;

        public override void OnSpawned(ItemData itemData, Cell newCell)
        {
            base.OnSpawned(itemData, newCell);
            sparkleParticle.PlayParticle(((ProductSo)Data.ItemSo).SparkParticleDuration);
        }

        public override void OnTap()
        {
            
        }

        public override void OnMerge()
        {
            base.OnMerge();
            sparkleParticle.StopParticle();
        }
    }
}
