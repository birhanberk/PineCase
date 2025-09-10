using System;
using System.Collections.Generic;
using Resources.Data;
using Resources.Type;
using UnityEngine;

namespace Resources
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private ResourceListSo resourceListSo;
        private readonly Dictionary<ResourceType, IResource> _resources = new();

        private void Awake()
        {
            _resources[ResourceType.Energy] = new RechargeableResource(resourceListSo.ResourceList[ResourceType.Energy], this);
            _resources[ResourceType.Coin] = new Resource(resourceListSo.ResourceList[ResourceType.Coin]);
        }

        public bool HasResource(ResourceType resourceType, int amount)
        {
            return GetAmount(resourceType) >= amount;
        }

        public int GetAmount(ResourceType type)
        {
            return _resources[type].Amount;
        }
        
        public bool TryGetResource(ResourceType type, out IResource resource)
        {
            return _resources.TryGetValue(type, out resource);
        }

        public void Spend(ResourceType type, int amount)
        {
            _resources[type].Spend(amount);
        }

        public void Add(ResourceType type, int amount)
        {
            _resources[type].Add(amount);
        }

        public void Subscribe(ResourceType type, Action<int> callback)
        {
            _resources[type].OnValueChanged += callback;
        }

        public void Unsubscribe(ResourceType type, Action<int> callback)
        {
            _resources[type].OnValueChanged -= callback;
        }
    }
}
