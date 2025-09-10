using Items.Type;
using UnityEngine;

namespace Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private BaseItem item;

        [SerializeField] private Vector2Int cellPosition;
        
        public Vector2Int CellPosition { get => cellPosition; set => cellPosition = value; }
        
        public void SetMaterial(Material material)
        {
            spriteRenderer.material = material;
        }

        public bool IsEmpty()
        {
            return item == null;
        }

        public void SetItem(BaseItem newItem)
        {
            item = newItem;
        }
    }
}