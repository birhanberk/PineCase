using System;
using Items;
using Items.Data;
using Items.Type;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Utils;

namespace UI.Orders
{
    public class OrderItemDisplay : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject readyImage;
        
        private ItemManager _itemManager;
        private ItemData _itemData;
        private bool _isReady;

        public bool IsReady => _isReady;
        public Action<bool> OnReadyChanged;

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
            _itemManager.OnItemAdded += HandleItemAdded;
            _itemManager.OnItemMerged += HandleItemAdded;
            _itemManager.OnItemRemoved += HandleItemRemoved;
        }

        private void RemoveListeners()
        {
            if (_itemManager == null) return;

            _itemManager.OnItemAdded -= HandleItemAdded;
            _itemManager.OnItemMerged -= HandleItemAdded;
            _itemManager.OnItemRemoved -= HandleItemRemoved;
        }

        public void SetItemData(ItemData itemData)
        {
            _itemData = itemData;

            image.sprite = _itemData.LevelSo.Sprite;
            image.preserveAspect = true;

            SetReady(_itemManager.HasItem(_itemData));
        }

        private void HandleItemAdded(BaseItem item)
        {
            if (Util.IsSameItem(item.Data, _itemData))
            {
                SetReady(true);
            }
        }

        private void HandleItemRemoved(BaseItem item)
        {
            if (Util.IsSameItem(item.Data, _itemData))
            {
                SetReady(_itemManager.HasItem(_itemData));
            }
        }

        private void SetReady(bool value)
        {
            _isReady = value;
            readyImage.SetActive(value);
            OnReadyChanged?.Invoke(value);
        }
    }
}