namespace Project.Infrastructure.Pool
{
    public interface IPoolableObject<T> where T : IPoolableObject<T>
    {
        public void OnCreated(IObjectPool<T> objectPool);
        public void ClearState();
    }
}