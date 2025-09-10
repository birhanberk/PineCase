using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Items.Data;
using Items.Handlers;
using Items.Type;
using UnityEngine;
using Utils;
using VContainer;

namespace Items
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private Transform itemParent;

        private ItemInputHandler _inputHandler;
        private readonly ItemSpawnHandler _itemSpawnHandler = new ();
        private readonly List<BaseItem> _items = new();

        public Action<BaseItem> OnItemSelectionStarted;
        public Action<BaseItem> OnItemSelected;
        public Action<BaseItem> OnItemMerged;
        public Action<BaseItem> OnItemAdded;
        public Action<BaseItem> OnItemRemoved;

        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            objectResolver.Inject(_itemSpawnHandler);
        }

        private void Start()
        {
            _inputHandler = new ItemInputHandler(RemoveItem, ItemMerged, ItemSelected, ItemSelectionStarted);

            if (IsInitialized())
            {
                LoadItems();
            }
        }
        
        private bool IsInitialized()
        {
            return itemParent.childCount > 0;
        }

        private void OnDestroy()
        {
            foreach (var item in _items)
                RemoveListeners(item);
        }

        private void LoadItems()
        {
            foreach (var item in itemParent.GetComponentsInChildren<BaseItem>())
            {
                AddItem(item);
                item.OnSpawned(item.Data, item.Cell);
                item.MoveInstant(item.Cell);
            }
        }

        public void CreateItem(ItemData itemData)
        {
            if (_itemSpawnHandler.CanSpawn())
            {
                var item = _itemSpawnHandler.SpawnItem(itemData, itemParent);
                AddItem(item);
            }
        }

        public bool CreateItem(ItemData itemData, Cell cell)
        {
            if (_itemSpawnHandler.CanSpawn())
            {
                var item = _itemSpawnHandler.SpawnItem(itemData, cell, itemParent);
                AddItem(item);
                return true;
            }

            return false;
        }

        private void AddItem(BaseItem item)
        {
            _items.Add(item);
            AddListeners(item);
            OnItemAdded?.Invoke(item);
        }

        private void RemoveItem(BaseItem item)
        {
            item.Cell.SetItem(null);
            _items.Remove(item);
            RemoveListeners(item);
            OnItemRemoved?.Invoke(item);
            _inputHandler.ItemRemoved(item);
            Destroy(item.gameObject);
        }

        public void RemoveItem(ItemData itemData)
        {
            var target = _items.Find(item => Util.IsSameItem(itemData, item.Data));
            if (target != null) RemoveItem(target);
        }

        private void ItemMerged(BaseItem item)
        {
            OnItemMerged?.Invoke(item);
        }

        private void ItemSelectionStarted(BaseItem item)
        {
            OnItemSelectionStarted?.Invoke(item);
        }

        private void ItemSelected(BaseItem item)
        {
            OnItemSelected?.Invoke(item);
        }

        private void AddListeners(BaseItem item)
        {
            item.OnInputDown += _inputHandler.HandleInputDown;
            item.OnInputDrag += _inputHandler.HandleInputDrag;
            item.OnInputUp += _inputHandler.HandleInputUp;
            item.OnRemoveItem += RemoveItem;
        }

        private void RemoveListeners(BaseItem item)
        {
            item.OnInputDown -= _inputHandler.HandleInputDown;
            item.OnInputDrag -= _inputHandler.HandleInputDrag;
            item.OnInputUp -= _inputHandler.HandleInputUp;
            item.OnRemoveItem -= RemoveItem;
        }

        public bool HasItem(ItemData compareItemData)
        {
            return _items.Any(item => Util.IsSameItem(compareItemData, item.Data));
        }
    }
}