using System;
using System.Collections.Generic;

namespace Project.Infrastructure.Pool
{
    public class GameObjectPool<T> : IObjectPool<T> where T : IPoolableObject<T>
    {
        private List<T> _allObjects;
        private Stack<T> _freeObjects;
        private HashSet<T> _busyObjects;
        private Func<T> _createFunc;

        public GameObjectPool(Func<T> createFunc)
        {
            _createFunc = createFunc;
            _freeObjects = new Stack<T>();
            _busyObjects = new HashSet<T>();
            _allObjects = new List<T>();
        }

        public IReadOnlyCollection<T> AllObjects => _allObjects.AsReadOnly();

        public void Dispose()
        {
            ReleaseAll();
        }

        public T Get()
        {
            var result = default(T);

            if (_freeObjects.Count == 0)
                result = Create();
            else
                result = _freeObjects.Pop();

            _busyObjects.Add(result);
            return result;
        }

        public void Release(T poolableObject)
        {
            if (_busyObjects.Remove(poolableObject))
            {
                poolableObject.ClearState();
                _freeObjects.Push(poolableObject);
            }
        }

        public void ReleaseAll()
        {
            while (_busyObjects.Count > 0)
            {
                var enumerator = _busyObjects.GetEnumerator();
                enumerator.MoveNext();
                var obj = enumerator.Current;
                Release(obj);
            }
        }

        private T Create()
        {
            var obj = _createFunc();
            obj.OnCreated(this);
            _allObjects.Add(obj);
            return obj;
        }
    }
}