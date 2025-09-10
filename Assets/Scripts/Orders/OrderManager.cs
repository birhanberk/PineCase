using Items;
using Resources;
using Resources.Type;
using UnityEngine;
using VContainer;

namespace Orders
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderSo orderSo;
        
        private ItemManager _itemManager;
        private ResourceManager _resourceManager;

        public OrderSo OrderSo => orderSo;
        
        [Inject]
        private void Construct(ItemManager itemManager, ResourceManager resourceManager)
        {
            _itemManager = itemManager;
            _resourceManager = resourceManager;
        }

        public void OnOrderCompleted(OrderData orderData)
        {
            foreach (var itemData in orderData.Items)
            {
                _itemManager.RemoveItem(itemData);
            }
            
            _resourceManager.Add(ResourceType.Coin, orderData.RewardAmount);
        }
    }
}
