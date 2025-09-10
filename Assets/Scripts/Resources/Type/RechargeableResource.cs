using System;
using System.Collections;
using Resources.Data;
using UnityEngine;

namespace Resources.Type
{
    public class RechargeableResource : IResource
    {
        private readonly int _maxAmount;
        private readonly float _rechargeInterval;
        private float _timer;
        private int _previousSeconds = -1;
        
        private Coroutine _rechargeCoroutine;
        private readonly ResourceManager _resourceManager;
        
        public int Amount { get; private set; }
        
        public event Action<int> OnValueChanged;
        public event Action<int> OnTimerChanged;

        public RechargeableResource(ResourceSo resourceSo, ResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
            Amount = resourceSo.InitialAmount;
            if (resourceSo is RechargeableResourceSo rechargeableResourceData)
            {
                _maxAmount = rechargeableResourceData.RechargeMaxAmount;
                _rechargeInterval = rechargeableResourceData.RechargeInterval;
            }
        }

        public void Add(int value)
        {
            Amount += value;
            OnValueChanged?.Invoke(Amount);
            
            if (IsFull())
            {
                StopRechargeCoroutine();
            }
        }

        public bool Spend(int value)
        {
            if (Amount < value) return false;
            Amount -= value;
            OnValueChanged?.Invoke(Amount);

            if (!IsFull())
            {
                StartRechargeCoroutine();
            }
            
            return true;
        }
        
        private void StartRechargeCoroutine()
        {
            _rechargeCoroutine ??= _resourceManager.StartCoroutine(RechargeRoutine());
        }

        private void StopRechargeCoroutine()
        {
            if (_rechargeCoroutine != null)
            {
                _resourceManager.StopCoroutine(_rechargeCoroutine);
                _rechargeCoroutine = null;
                _timer = 0f;
                _previousSeconds = -1;
            }
        }
        
        private IEnumerator RechargeRoutine()
        {
            while (!IsFull())
            {
                _timer = 0f;
                _previousSeconds = -1;

                while (_timer < _rechargeInterval)
                {
                    _timer += Time.deltaTime;

                    var currentSeconds = Mathf.CeilToInt(TimeUntilNextRecharge());
                    if (currentSeconds != _previousSeconds)
                    {
                        OnTimerChanged?.Invoke(currentSeconds);
                        _previousSeconds = currentSeconds;
                    }

                    yield return null;
                }

                Add(1);
            }

            StopRechargeCoroutine();
        }

        public float TimeUntilNextRecharge()
        {
            return _rechargeInterval - _timer;
        }

        public bool IsFull()
        {
            return Amount >= _maxAmount;
        }
    }
}