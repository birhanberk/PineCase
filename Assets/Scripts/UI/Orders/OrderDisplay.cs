using System;
using System.Collections.Generic;
using System.Linq;
using Orders;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Orders
{
    public class OrderDisplay : MonoBehaviour
    {
        [SerializeField] private Transform orderItemParent;
        [SerializeField] private OrderItemDisplay orderItemPrefab;
        [SerializeField] private Button serveButton;
        
        private readonly List<OrderItemDisplay> _orderItems = new();
        private IObjectResolver _objectResolver;
        private OrderData _orderData;
        
        public Action<OrderDisplay> OnServePerformed;

        public OrderData Data => _orderData;

        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
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
            serveButton.onClick.AddListener(OnServeButtonPerformed);
        }
        
        private void RemoveListeners()
        {
            serveButton.onClick.RemoveListener(OnServeButtonPerformed);
            foreach (var orderItem in _orderItems)
            {
                orderItem.OnReadyChanged -= OnReadyChanged;
            }
        }

        public void SetData(OrderData orderData)
        {
            _orderData = orderData;
            for (var i = 0; i < 2; i++)
            {
                if (i < _orderData.Items.Count)
                {
                    var newOrderItem = Instantiate(orderItemPrefab, orderItemParent);
                    _orderItems.Add(newOrderItem);
                    _objectResolver.Inject(newOrderItem);
                    newOrderItem.OnReadyChanged += OnReadyChanged;
                    newOrderItem.SetItemData(_orderData.Items[i]);
                }
            }
        }
        
        private void OnReadyChanged(bool value)
        {
            if (!value)
            {
                serveButton.gameObject.SetActive(false);
                return;
            }

            var allReady = _orderItems.Where(item => item.isActiveAndEnabled).All(item => item.IsReady);
            serveButton.gameObject.SetActive(allReady);
        }
        
        private void OnServeButtonPerformed()
        {
            OnServePerformed?.Invoke(this);
        }
    }
}
