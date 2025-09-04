using System;

namespace Project.Core.Common
{
    public interface IHealth
    {
        public event Action<float, float> Changed;
        public event Action Exhausted;

        public float Total { get; }
        public float Current { get; }

        public void SetTotal(float total);
        public void SetCurrent(float current);
        public void Remove(float amount);
    }
}
