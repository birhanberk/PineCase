using Items;
using Items.Type;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI.ItemInfo
{
    public class ItemInfoPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text itemDescriptionText;

        private ItemManager _itemManager;
        
        [Inject]
        private void Construct(ItemManager itemManager)
        {
            _itemManager = itemManager;
        }

        private void Start()
        {
            AddListeners();
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void AddListeners()
        {
            _itemManager.OnItemSelectionStarted += UpdateVisuals;
            _itemManager.OnItemSelected += UpdateVisuals;
        }

        private void RemoveListeners()
        {
            _itemManager.OnItemSelectionStarted -= UpdateVisuals;
            _itemManager.OnItemSelected -= UpdateVisuals;
        }

        private void UpdateVisuals(BaseItem item)
        {
            if (item != null)
            {
                var itemData = item.Data;
                itemNameText.text = itemData.LevelSo.ItemName + " (Lvl " + itemData.LevelSo.Level + ")";
                itemDescriptionText.text = itemData.LevelSo.Description;
            }
        }
    }
}
