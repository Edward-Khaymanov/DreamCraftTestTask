using System.Collections.Generic;
using UnityEngine;

namespace Project.Core.UI
{
    public class WindowsRepository
    {
        private List<IUIWindow> _windows;

        public WindowsRepository()
        {
            _windows = new List<IUIWindow>();
        }

        public void Register<T>(T window) where T : IUIWindow, new()
        {
            var found = TryGetValue<T>(out var oldWindow);
            if (found)
            {
                Debug.LogError($"Window with type {typeof(T)} already registered");
                return;
            }
            _windows.Add(window);
        }

        public void Unregister<T>() where T : IUIWindow, new()
        {
            var found = TryGetValue<T>(out var window);
            if (found)
            {
                Debug.LogError($"Window with type {typeof(T)} already not registered");
                return;
            }
            _windows.Remove(window);
        }

        public bool TryGetValue<T>(out T value) where T : IUIWindow, new()
        {
            value = (T)_windows.Find(x => x is T);
            return value != null;
        }
    }
}