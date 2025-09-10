using UnityEngine;

namespace Items.Data.Producer
{
    [CreateAssetMenu(fileName = "Producer Item", menuName = "Data / Item / Type / Producer Item")]
    public class ProducerSo : ItemSo
    {
        [SerializeField] private Sprite rechargeSprite;
        [SerializeField] private int consumeAmount;

        public Sprite RechargeSprite => rechargeSprite;
        public int ConsumeAmount => consumeAmount;
    }
}
