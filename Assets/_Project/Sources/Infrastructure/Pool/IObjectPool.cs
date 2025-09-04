using System;
using System.Collections.Generic;

namespace Project.Infrastructure.Pool
{
    public interface IObjectPool<T> : IDisposable where T : IPoolableObject<T>
    {
        public IReadOnlyCollection<T> AllObjects { get; }

        public T Get();
        public void Release(T poolableObject);
        public void ReleaseAll();
    }
}