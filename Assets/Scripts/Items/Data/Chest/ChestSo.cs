using UnityEngine;

namespace Items.Data.Chest
{
    [CreateAssetMenu(fileName = "Chest Item", menuName = "Data / Item / Type / Chest Item")]
    public class ChestSo : ItemSo
    {
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Sprite unlockingSprite;
        [SerializeField] private float unlockTime;

        public Sprite LockedSprite => lockedSprite;
        public Sprite UnlockingSprite => unlockingSprite;
        public float UnlockTime => unlockTime;
    }
}
