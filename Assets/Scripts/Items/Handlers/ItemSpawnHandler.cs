using Grid;
using Items.Data;
using Items.Type;
using UnityEngine;
using VContainer;

namespace Items.Handlers
{
    public class ItemSpawnHandler
    {
        private IObjectResolver _objectResolver;
        private GridManager _gridManager;
        
        [Inject]
        private void Construct(IObjectResolver objectResolver, GridManager gridManager)
        {
            _objectResolver = objectResolver;
            _gridManager = gridManager;
        }

        public bool CanSpawn()
        {
            return _gridManager.HasEmptyCell();
        }
        
        public BaseItem SpawnItem(ItemData itemData, Transform parent)
        {
            var item = CreateItem(itemData, parent);
            _objectResolver.Inject(item);
            var cell = _gridManager.GetRandomEmptyCell();
            item.OnSpawned(new ItemData(itemData), cell);
            item.MoveInstant(cell);
            cell.SetItem(item);
            return item;
        }
        
        public BaseItem SpawnItem(ItemData itemData, Cell targetCell, Transform parent)
        {
            var item = CreateItem(itemData, parent);
            _objectResolver.Inject(item);
            var cell = _gridManager.GetEmptyCell(targetCell);
            item.OnSpawned(new ItemData(itemData), cell);
            item.Pop(targetCell, cell);
            cell.SetItem(item);
            return item;
        }

        private BaseItem CreateItem(ItemData itemData, Transform parent)
        {
            return Object.Instantiate(itemData.ItemSo.Prefab, parent);
        } 
    }
}
