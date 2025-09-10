using System.Collections;
using System.Collections.Generic;
using Grid;
using Items.Data;
using Items.Data.Common;
using Items.Data.Producer;
using Resources;
using Resources.Type;
using UnityEngine;
using Utils;
using VContainer;

namespace Items.Type.Producer
{
    public class ProducerItem : BaseItem
    {
        [SerializeField] private Indicator indicator;
        [SerializeField] private ItemParticlePlayer glowParticle;

        private ItemManager _itemManager;
        private ResourceManager _resourceManager;
        
        private ProducerState _producerState;
        private readonly List<ProduceData> _produceList = new ();

        [Inject]
        private void Construct(ItemManager itemManager, ResourceManager resourceManager)
        {
            _itemManager = itemManager;
            _resourceManager = resourceManager;
        }

        public override void OnSpawned(ItemData itemData, Cell newCell)
        {
            base.OnSpawned(itemData, newCell);
            SetState(ProducerState.Active);
        }
        
        public override void OnMerge()
        {
            base.OnMerge();
            SetState(ProducerState.Active);
        }
        
        private void PopulateStack()
        {
            _produceList.Clear();
            foreach (var produceData in ((ProducerLevelSo)Data.LevelSo).ProduceData)
            {
                for (var i = 0; i < produceData.Capacity; i++)
                {
                    _produceList.Add(produceData);
                }
            }
            _produceList.Shuffle();
        }
        
        public override void OnTap()
        {
            switch (_producerState)
            {
                case ProducerState.Active:
                    PopItem();
                    break;
                case ProducerState.Recharging:
                    break;
            }
        }
        
        private void PopItem()
        {
            var consumeAmount = ((ProducerSo)Data.ItemSo).ConsumeAmount;
            if (_produceList.Count > 0 && _resourceManager.HasResource(ResourceType.Energy, consumeAmount))
            {
                var produceData = _produceList[0];
                var result = _itemManager.CreateItem(produceData.Data, Cell);
                if (result)
                {
                    _produceList.RemoveAt(0);
                    _resourceManager.Spend(ResourceType.Energy, consumeAmount);
                    if (_produceList.Count == 0)
                    {
                        SetState(ProducerState.Recharging);
                    }
                }
            }
        }
        
        private IEnumerator Recharge()
        {
            var rechargeTime = ((ProducerLevelSo)Data.LevelSo).RechargeTime;
            var elapsed = 0f;
            indicator.SetFillAmount(0f);
            while (elapsed < rechargeTime)
            {
                elapsed += Time.deltaTime;
                var fill = Mathf.Clamp01(elapsed / rechargeTime);
                indicator.SetFillAmount(fill);
                yield return null;
            }

            SetState(ProducerState.Active);
        }

        private void SetState(ProducerState state)
        {
            _producerState = state;
            switch (_producerState)
            {
                case ProducerState.Active:
                    OnActiveState();
                    break;
                case ProducerState.Recharging:
                    OnRechargingState();
                    break;
            }
        }

        private void OnActiveState()
        {
            PopulateStack();
            indicator.Hide();
            if (_produceList.Count > 0)
            {
                glowParticle.PlayParticle();
            }
        }

        private void OnRechargingState()
        {
            indicator.Show(((ProducerSo)Data.ItemSo).RechargeSprite);
            StartCoroutine(Recharge());
            glowParticle.StopParticle();
        }
    }
}
