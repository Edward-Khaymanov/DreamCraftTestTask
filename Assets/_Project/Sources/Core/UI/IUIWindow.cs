namespace Project.Core.UI
{
    public interface IUIWindow
    {
        public bool IsVisible { get; }
        public void Show();
        public void Hide();
    }
}