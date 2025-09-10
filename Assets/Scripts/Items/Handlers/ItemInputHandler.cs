using System;
using Grid;
using Items.Type;
using UnityEngine;

namespace Items.Handlers
{
    [Serializable]
    public class ItemInputHandler
    {
        private readonly Camera _camera;
        private readonly ItemMergeHandler _mergeHandler;

        private BaseItem _selectedItem;
        private bool _isDragged;

        private readonly Action<BaseItem> _onItemSelected;
        private readonly Action<BaseItem> _onItemSelectionStart;

        public ItemInputHandler(Action<BaseItem> onItemRemoved, Action<BaseItem> onItemMerged, Action<BaseItem> onItemSelected, Action<BaseItem> onItemSelectionStart)
        {
            _camera = Camera.main;
            _onItemSelected = onItemSelected;
            _onItemSelectionStart = onItemSelectionStart;
            _mergeHandler = new ItemMergeHandler(onItemRemoved, onItemMerged);
        }

        public void HandleInputDown(BaseItem item)
        {
            _onItemSelectionStart?.Invoke(item);
        }

        public void HandleInputDrag(BaseItem item)
        {
            _isDragged = true;
            _onItemSelected?.Invoke(null);
        }

        public void HandleInputUp(BaseItem item)
        {
            if (!_isDragged)
            {
                item.PlayTapAnimation();
                _onItemSelected?.Invoke(item);
                if (_selectedItem != null && _selectedItem == item)
                {
                    item.OnTap();
                }
            }
            else
            {
                CheckInteraction(item);
            }
            _selectedItem = item;
            _isDragged = false;
        }

        private void CheckInteraction(BaseItem item)
        {
            var worldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(worldPos, Vector2.zero).collider;

            if (hit == null)
            {
                item.ReturnToCell();
                _onItemSelected?.Invoke(item);
                return;
            }

            if (hit.TryGetComponent(out Cell targetCell) && targetCell != item.Cell)
            {
                DropOnCell(item, targetCell);
            }
            else if (hit.TryGetComponent(out BaseItem targetItem))
            {
                DropOnItem(item, targetItem);
            }
            else
            {
                item.ReturnToCell();
            }

            _onItemSelected?.Invoke(item);
        }

        private void DropOnCell(BaseItem draggedItem, Cell targetCell)
        {
            if (targetCell.IsEmpty())
            {
                MoveItem(draggedItem, targetCell);
            }
            else
            {
                draggedItem.ReturnToCell();
            }
        }

        private void DropOnItem(BaseItem draggedItem, BaseItem targetItem)
        {
            if (_mergeHandler.CanMerge(draggedItem, targetItem))
            {
                _mergeHandler.Merge(draggedItem, targetItem, MoveItem);
            }
            else
            {
                var originalCell = targetItem.Cell;
                MoveItem(targetItem, draggedItem.Cell);
                MoveItem(draggedItem, originalCell);
            }
        }

        private void MoveItem(BaseItem item, Cell targetCell)
        {
            item.Cell.SetItem(null);
            item.Move(targetCell);
            targetCell.SetItem(item);
        }

        public void ItemRemoved(BaseItem item)
        {
            if (item == _selectedItem)
            {
                _onItemSelected?.Invoke(null);
            }
        }
    }
}
