using System;
using Resources.Data;

namespace Resources.Type
{
    public class Resource : IResource
    {
        public int Amount { get; private set; }
        
        public event Action<int> OnValueChanged;

        public Resource(ResourceSo so)
        {
            Amount = so.InitialAmount;
        }

        public void Add(int value)
        {
            Amount += value;
            OnValueChanged?.Invoke(Amount);
        }

        public bool Spend(int value)
        {
            if (Amount < value) return false;
            Amount -= value;
            OnValueChanged?.Invoke(Amount);
            return true;
        }
    }
}