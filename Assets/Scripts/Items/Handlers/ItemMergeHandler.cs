using System;
using Grid;
using Items.Type;
using Utils;

namespace Items.Handlers
{
    [Serializable]
    public class ItemMergeHandler
    {
        private readonly Action<BaseItem> _onItemRemoved;
        private readonly Action<BaseItem> _onItemMerged;

        public ItemMergeHandler(Action<BaseItem> onItemRemoved, Action<BaseItem> onItemMerged)
        {
            _onItemRemoved = onItemRemoved;
            _onItemMerged = onItemMerged;
        }

        public bool CanMerge(BaseItem a, BaseItem b)
        {
            return Util.IsSameItem(a.Data, b.Data) && a.Data.LevelSo.Level < a.Data.ItemSo.MaxLevel;
        }

        public void Merge(BaseItem draggedItem, BaseItem targetItem, Action<BaseItem, Cell> moveItem)
        {
            var targetCell = targetItem.Cell;

            moveItem(draggedItem, targetCell);

            draggedItem.OnMerge();
            _onItemMerged?.Invoke(draggedItem);
            _onItemRemoved?.Invoke(targetItem);
            targetCell.SetItem(draggedItem);
        }
    }
}
