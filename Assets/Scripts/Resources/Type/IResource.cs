using System;

namespace Resources.Type
{
    public interface IResource
    {
        int Amount { get; }
        void Add(int value);
        bool Spend(int value);
        event Action<int> OnValueChanged;
    }
}