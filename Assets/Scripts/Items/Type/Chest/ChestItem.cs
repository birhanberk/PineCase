using System.Collections;
using System.Collections.Generic;
using Grid;
using Items.Data;
using Items.Data.Chest;
using Items.Data.Common;
using UnityEngine;
using VContainer;

namespace Items.Type.Chest
{
    public class ChestItem : BaseItem
    {
        [SerializeField] private Indicator indicator;
        
        private ItemManager _itemManager;
        private ChestState _chestState;
        
        private Coroutine _unlockedCoroutine;
        private readonly List<ProduceData> _chestList = new ();

        [Inject]
        private void Construct(ItemManager itemManager)
        {
            _itemManager = itemManager;
        }

        public override void OnSpawned(ItemData itemData, Cell newCell)
        {
            base.OnSpawned(itemData, newCell);
            SetState(ChestState.Locked);
        }

        public override void OnMerge()
        {
            base.OnMerge();
            SetState(ChestState.Locked);
            StopUnlockedCoroutine();
            indicator.SetFillAmount(1f);
        }

        public override void OnTap()
        {
            switch (_chestState)
            {
                case ChestState.Locked:
                    _unlockedCoroutine = StartCoroutine(WaitForUnlockedTime());
                    break;
                case ChestState.Unlocking:
                    break;
                case ChestState.Unlocked:
                    PopItem();
                    break;
            }
        }
        
        private void PopulateStack()
        {
            foreach (var produceData in ((ChestLevelSo)Data.LevelSo).ProduceData)
            {
                for (var i = 0; i < produceData.Capacity; i++)
                {
                    _chestList.Add(produceData);
                }
            }
        }

        private IEnumerator WaitForUnlockedTime()
        {
            SetState(ChestState.Unlocking);

            var unlockDuration = ((ChestSo)Data.ItemSo).UnlockTime;
            var elapsed = 0f;
            indicator.SetFillAmount(0f);
            while (elapsed < unlockDuration)
            {
                elapsed += Time.deltaTime;
                var fill = Mathf.Clamp01(elapsed / unlockDuration);
                indicator.SetFillAmount(fill);
                yield return null;
            }

            SetState(ChestState.Unlocked);
        }

        private void StopUnlockedCoroutine()
        {
            if (_unlockedCoroutine != null)
            {
                StopCoroutine(_unlockedCoroutine);
                _unlockedCoroutine = null;
            }
        }

        private void PopItem()
        {
            if (_chestList.Count > 0)
            {
                var produceData = _chestList[0];
                
                var result = _itemManager.CreateItem(produceData.Data, Cell);
                if (result)
                {
                    _chestList.RemoveAt(0);
                    if (_chestList.Count == 0)
                    {
                        DestroyItem();
                    }
                }
            }
        }

        private void SetState(ChestState state)
        {
            _chestState = state;
            var chestSo = (ChestSo)Data.ItemSo;
            switch (_chestState)
            {
                case ChestState.Locked:
                    indicator.Show(chestSo.LockedSprite);
                    PopulateStack();
                    break;
                case ChestState.Unlocking:
                    indicator.Show(chestSo.UnlockingSprite);
                    break;
                case ChestState.Unlocked:
                    indicator.Hide();
                    break;
            }
        }
    }
}
