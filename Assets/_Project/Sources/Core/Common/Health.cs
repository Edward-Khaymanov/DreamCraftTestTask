using System;
using UnityEngine;

namespace Project.Core.Common
{
    public class Health : IHealth
    {
        public event Action<float, float> Changed;
        public event Action Exhausted;

        public float Total { get; private set; }
        public float Current { get; private set; }

        public void SetTotal(float total)
        {
            Total = total;
        }

        public void SetCurrent(float current)
        {
            Current = Mathf.Clamp(current, 0, Total);
        }

        public void Remove(float amount)
        {
            amount = Mathf.Abs(amount);
            Current = Mathf.Clamp(Current - amount, 0, float.MaxValue);
            Changed?.Invoke(Current, Total);

            if (Current == 0)
            {
                Exhausted?.Invoke();
            }
        }
    }
}
