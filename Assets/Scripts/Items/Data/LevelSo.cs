using UnityEngine;

namespace Items.Data
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data / Item / Level / Base Level")]
    public class LevelSo : ScriptableObject
    {
        [SerializeField] private int level;
        [SerializeField] private string itemName;
        [SerializeField] private string description;
        [SerializeField] private Sprite sprite;

        public int Level => level;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Sprite => sprite;
    }
}
