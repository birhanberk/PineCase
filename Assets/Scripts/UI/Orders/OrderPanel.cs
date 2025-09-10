using System.Collections.Generic;
using Orders;
using UnityEngine;
using VContainer;

namespace UI.Orders
{
    public class OrderPanel : MonoBehaviour
    {
        [SerializeField] private Transform orderItemsParent;
        [SerializeField] private OrderDisplay orderPrefab;
        
        private IObjectResolver _objectResolver;
        private OrderManager _orderManager;
        
        private List<OrderDisplay> _orders = new ();
        private int _loadedOrderCount;
        
        [Inject]
        private void Construct(IObjectResolver objectResolver, OrderManager orderManager)
        {
            _objectResolver = objectResolver;
            _orderManager = orderManager;
        }

        private void Start()
        {
            CreateOrders();
        }

        private void CreateOrders()
        {
            var orderSo = _orderManager.OrderSo;
            var orderCount = Mathf.Min(orderSo.Orders.Count, orderSo.DisplayCount);
            for (var i = 0; i < orderCount; i++)
            {
                CreateOrder(orderSo.Orders[i]);
            }
        }

        private void CreateOrder(OrderData orderData)
        {
            _loadedOrderCount++;
            var order = Instantiate(orderPrefab, orderItemsParent);
            _orders.Add(order);
            _objectResolver.Inject(order);
            order.OnServePerformed += OnOrderCompleted;
            order.SetData(orderData);
        }

        private void OnOrderCompleted(OrderDisplay order)
        {
            RemoveOrder(order);
            
            var orderSo = _orderManager.OrderSo;
            if (orderSo.Orders.Count > _loadedOrderCount && orderSo.Orders.Count >= _orderManager.OrderSo.DisplayCount)
            {
                CreateOrder(orderSo.Orders[_loadedOrderCount]);
            }
        }

        private void RemoveOrder(OrderDisplay order)
        {
            order.OnServePerformed -= OnOrderCompleted;
            _orders.Remove(order);
            Destroy(order.gameObject);
            _orderManager.OnOrderCompleted(order.Data);
        }
    }
}
