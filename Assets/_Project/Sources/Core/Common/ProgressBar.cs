using UnityEngine;
using UnityEngine.UI;

namespace Project.Core.Common
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _progressSource;

        public void Fill(float current, float total)
        {
            total = Mathf.Max(total, 0f);
            current = Mathf.Clamp(current, 0f, total);

            var amount = current / total;
            if (current == 0f)
            {
                amount = 0f;
            }

            _progressSource.fillAmount = amount;
        }
    }
}