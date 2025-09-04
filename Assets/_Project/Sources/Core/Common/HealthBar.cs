using System;

namespace Project.Core.Common
{
    public class HealthBar : ProgressBar, IDisposable
    {
        private IHealth _health;

        public void Init(IHealth health)
        {
            _health = health;
            _health.Changed += UpdateHealth;
            UpdateHealth(_health.Current, _health.Total);
        }

        public void Dispose()
        {
            _health.Changed -= UpdateHealth;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void UpdateHealth(float current, float total)
        {
            Fill(current, total);
        }
    }
}