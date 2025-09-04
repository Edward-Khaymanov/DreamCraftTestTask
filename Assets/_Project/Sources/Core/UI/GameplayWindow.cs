using Project.Core.Common;
using UnityEngine;

namespace Project.Core.UI
{
    public class GameplayWindow : MonoBehaviour, IUIWindow
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private HealthBar _healthBar;

        public HealthBar HealthBar => _healthBar;
        public bool IsVisible => _canvas.enabled;

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}